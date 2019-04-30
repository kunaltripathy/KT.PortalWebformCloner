using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KT.PortalsWebformCloner.AppCode;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Args;
using XrmToolBox.Extensibility.Interfaces;

namespace KT.PortalsWebformCloner
{
    public partial class MyPluginControl : PluginControlBase , IStatusBarMessenger, IHelpPlugin, IGitHubPlugin
    {
        #region IHelpPlugin implementation
        public string HelpUrl => "https://kunaltripathy.com";

        #endregion IHelpPlugin implementation

        #region IGitHubPlugin implementation
        public string RepositoryName => "KT.PortalsWebformCloner";
        public string UserName => "kunaltripathy";
        #endregion IGitHubPlugin implementation

        private Settings _mySettings;
        private readonly LogManager _logger;

        public event EventHandler<StatusBarMessageEventArgs> SendMessageToStatusBar;

        public MyPluginControl()
        {
            InitializeComponent();
            _logger = new LogManager(GetType());
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out _mySettings))
            {
                _mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        public Guid SelectedWebformId
        {
            get => ((Webform)cbWebformToClone.SelectedItem)?.Record.Id ?? Guid.Empty;
            set
            {
                foreach (Webform webform in cbWebformToClone.Items)
                {
                    if (webform.Record.Id == value)
                    {
                        cbWebformToClone.SelectedItem = webform;
                    }
                }

            }
        }

        public Entity SelectedWebform;

        public Guid ClonedWebformId { get; set; }

        public Guid ClonedWebformFirstStepId { get; set; }

        #region Events

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), _mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (_mySettings != null && detail != null)
            {
                _mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        private void toolStripMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tsbLoadWebforms_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadItems);
        }
        private void cbWebformToClone_SelectedIndexChanged(object sender, EventArgs e)
        {
            txWebformName.Enabled = true;
            cbReadonly.Enabled = true;

        }

        private void txWebformName_TextChanged(object sender, EventArgs e)
        {
            tsbCloneWebform.Enabled = true;
        }

        private void tsbCloneWebform_Click(object sender, EventArgs e)
        {
            tsbCloneWebform.Enabled = false;
            var cloneSetting = new CloneSetting
            {
                SelectedWebformId = SelectedWebformId,
                ReadOnlyMode = cbReadonly.Checked,
                WebformName = txWebformName.Text
            };

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Cloning Webform...",
                AsyncArgument = cloneSetting,
                Work = (bw, evt) =>
                {
                    var wcm = new WebformCloneManager(Service);
                    _logger.LogInfo("Initiate Cloning of Webform");
                    bw.ReportProgress(0, "Cloning WebForm");
                    evt.Cancel = wcm.CloneWebform((CloneSetting)evt.Argument, Service, bw);

                    _logger.LogInfo("Initiate Cloning of Webformsteps");
                    bw.ReportProgress(40, "Cloning WebFormStep");

                    evt.Cancel = wcm.CloneWebformStep((CloneSetting)evt.Argument, Service, bw);

                    _logger.LogInfo("update webformstep references");
                    bw.ReportProgress(80, "Updating References");

                    evt.Cancel = wcm.UpdateStepReferences((CloneSetting)evt.Argument, Service, bw);
                },
                PostWorkCallBack = evt =>
                {
                    if (evt.Error != null)
                    {
                        MessageBox.Show(this, $"An error occured: {evt.Error.Message}", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                },
                ProgressChanged = evt =>
                {
                    SetWorkingMessage(evt.UserState.ToString());
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(evt.UserState.ToString()));
                }
            });

            tsbCloneWebform.Enabled = true;
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        #endregion Events

        #region Methods

        public void LoadItems()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Webforms...",
                Work = (bw, evt) =>
                {

                    bw.ReportProgress(0, "Loading WebForms");
  
                    evt.Result = Service.RetrieveMultiple(new QueryExpression("adx_webform")
                    {
                        ColumnSet = new ColumnSet(true)
                    }).Entities.Select(record => new Webform(record)).ToList();
                },
                PostWorkCallBack = evt =>
                {
                    if (evt.Error != null)
                    {
                        MessageBox.Show(this, $"An error occured: {evt.Error.Message}", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    cbWebformToClone.Items.Clear();
                    cbWebformToClone.Items.AddRange(((List<Webform>)evt.Result).ToArray());

                    cbWebformToClone.Enabled = true;
                    
                },
                ProgressChanged = evt =>
                {
                    SetWorkingMessage(evt.UserState.ToString());
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(evt.UserState.ToString()));
                }
            });
        }

        #endregion Methods
    }
}