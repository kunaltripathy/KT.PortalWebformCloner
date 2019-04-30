using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using XrmToolBox.Extensibility;

namespace KT.PortalsWebformCloner.AppCode
{
    internal class WebformCloneManager
    {
        private readonly LogManager _logger;
        private Entity _webFormToClone;
        private readonly IOrganizationService _service;
        private Guid _clonedWebformFirstStepId;
        private Guid _clonedWebformId;
        //Dictionary with Source Guid and target guid of webform
        private Dictionary<Guid, Guid> webformStepGuids = new Dictionary<Guid, Guid>();
        private List<Entity> _webFormSteps;

        public WebformCloneManager(IOrganizationService service)
        {
            _service = service;
            _logger = new LogManager(GetType());
           
        }

        public bool UpdateStepReferences(CloneSetting cs, IOrganizationService service, BackgroundWorker worker)
        {
            try
            {

                //update webformstep references
                _logger.LogInfo("update Webform step reference");
                foreach (var step in _webFormSteps)
                {
                   Guid clonedStepId;
                    webformStepGuids.TryGetValue(step.Id, out clonedStepId);
                        var clonedStep = new Entity("adx_webformstep", clonedStepId);

                    if (step.GetAttributeValue<EntityReference>("adx_nextstep") != null)
                    {
                        clonedStep["adx_nextstep"] = new EntityReference("adx_webformstep", webformStepGuids[step.GetAttributeValue<EntityReference>("adx_nextstep").Id]);
                    }
                    if (step.GetAttributeValue<EntityReference>("adx_entitysourcestep") != null)
                    {
                        clonedStep["adx_entitysourcestep"] = new EntityReference("adx_webformstep", webformStepGuids[step.GetAttributeValue<EntityReference>("adx_entitysourcestep").Id]);
                    }
                    if (step.GetAttributeValue<EntityReference>("adx_conditiondefaultnextstep") != null)
                    {
                        clonedStep["adx_conditiondefaultnextstep"] = new EntityReference("adx_webformstep", webformStepGuids[step.GetAttributeValue<EntityReference>("adx_conditiondefaultnextstep").Id]);
                    }
                    if (step.GetAttributeValue<EntityReference>("adx_referenceentitystep") != null)
                    {
                        clonedStep["adx_referenceentitystep"] = new EntityReference("adx_webformstep", webformStepGuids[step.GetAttributeValue<EntityReference>("adx_referenceentitystep").Id]);
                    }
                    if (step.GetAttributeValue<EntityReference>("adx_previousstep") != null)
                    {
                        clonedStep["adx_previousstep"] = new EntityReference("adx_webformstep", webformStepGuids[step.GetAttributeValue<EntityReference>("adx_previousstep").Id]);
                    }

                    _service.Update(clonedStep);

                }


                //if webform has 1st step
                _logger.LogInfo("update 1st step on Webform");
                if (_clonedWebformFirstStepId != Guid.Empty)
                {
                    //update 1st step reference
                    var clonedWebform = new Entity("adx_webform", _clonedWebformId)
                    {
                        ["adx_startstep"] = new EntityReference("adx_webformstep", _clonedWebformFirstStepId)
                    };


                    _service.Update(clonedWebform);
                }
                worker.ReportProgress(95, true);
            }
            catch (Exception error)
            {
                _logger.LogInfo(error.Message);
                var percentage = 95;
                worker.ReportProgress(percentage, false);
            }
            return false;
        }

        private void CloneWebformMetadata(Entity sourceStep, Guid targetStepId)
        {
            _logger.LogInfo("Retrieve Webform metadata");
            var webFormStepMetadatas = _service.RetrieveMultiple(new QueryExpression("adx_webformmetadata")
            {
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("adx_webformstep", ConditionOperator.Equal, sourceStep.Id)
                    }
                },
                ColumnSet = new ColumnSet(true)
            }).Entities.ToList();

            _logger.LogInfo("Clone Webform metadata");
            foreach (var webformstepmetadata in webFormStepMetadatas)
            {
                Entity newWebformMetadata = new Entity("adx_webformmetadata");

                //copy all attributes.
                foreach (var v in webformstepmetadata.Attributes)
                {
                    newWebformMetadata.Attributes.Add(v.Key, v.Value);
                }

                //Remove attributed not to be copied
                newWebformMetadata.Attributes.Remove("adx_webformmetadataid");

                //

                //Change reference for webform step 
                newWebformMetadata["adx_webformstep"] = new EntityReference("adx_webformstep", targetStepId);

                //create webform
                 _service.Create(newWebformMetadata);

            }
        }

        public bool CloneWebformStep(CloneSetting cs, IOrganizationService service, BackgroundWorker worker)
        {
            try
            {
                _logger.LogInfo("Retrieve Webform Steps");
                _webFormSteps = _service.RetrieveMultiple(new QueryExpression("adx_webformstep")
                {
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("adx_webform", ConditionOperator.Equal, cs.SelectedWebformId)
                        }
                    },
                    ColumnSet = new ColumnSet(true)
                }).Entities.ToList();

                _logger.LogInfo("Clone Webform Steps");
                foreach (var webFormStep in _webFormSteps)
                {
                    Entity newWebformstep = new Entity("adx_webformstep");

                    //copy all attributes.
                    foreach (var v in webFormStep.Attributes)
                    {
                        newWebformstep.Attributes.Add(v.Key, v.Value);
                    }

                    //Remove attributed not to be copied
                    newWebformstep.Attributes.Remove("adx_webformstepid");
                    //
                    newWebformstep.Attributes.Remove("adx_nextstep");
                    newWebformstep.Attributes.Remove("adx_entitysourcestep");
                    newWebformstep.Attributes.Remove("adx_conditiondefaultnextstep");
                    newWebformstep.Attributes.Remove("adx_referenceentitystep");
                    newWebformstep.Attributes.Remove("adx_previousstep");

                    //change reference to webformstep TODO
                    newWebformstep["adx_webform"] = new EntityReference("adx_webform", _clonedWebformId);

                    //handle first step mode change if applicable
                    if (webFormStep.GetAttributeValue<OptionSetValue>("adx_type") != null
                        && (webFormStep.GetAttributeValue<OptionSetValue>("adx_type").Value == 100000001 ||
                            webFormStep.GetAttributeValue<OptionSetValue>("adx_type").Value == 100000002)
                        && cs.ReadOnlyMode)
                    {
                        newWebformstep["adx_mode"] = new OptionSetValue(100000002);

                    }

                    //Create webformStep
                    var clonedWebformstepId = _service.Create(newWebformstep);

                    if (_webFormToClone.GetAttributeValue<EntityReference>("adx_startstep") != null && webFormStep.Id ==
                        _webFormToClone.GetAttributeValue<EntityReference>("adx_startstep").Id)
                    {
                        _clonedWebformFirstStepId = clonedWebformstepId;
                    }
                    //add guid to map
                    webformStepGuids.Add(webFormStep.Id, clonedWebformstepId);

                    //Clone webformstepMetdata
                    CloneWebformMetadata(webFormStep, clonedWebformstepId);
                }
            }
            catch (Exception error)
            {
                _logger.LogInfo(error.Message);
                var percentage = 85;
                worker.ReportProgress(percentage, false);
            }
            return false;
            

        }

        public bool CloneWebform(CloneSetting cs, IOrganizationService service, BackgroundWorker worker)
        {
            //retrieve selected Webform
            _webFormToClone = service.Retrieve("adx_webform", cs.SelectedWebformId, new ColumnSet(true));

            try
            {
                //Create new Webform
                Entity newWebform = new Entity("adx_webform");

                //copy all attributes.
                foreach (var v in _webFormToClone.Attributes)
                {
                    newWebform.Attributes.Add(v.Key, v.Value);
                }

                //Remove attributed not to be copied
                newWebform.Attributes.Remove("adx_webformid");
                newWebform.Attributes.Remove("adx_startstep");

                //Set the name of the webform
                newWebform["adx_name"] = String.IsNullOrEmpty(cs.WebformName)
                    ? _webFormToClone.GetAttributeValue<string>("adx_name") + "  - Copy " +
                      DateTime.Now.ToShortTimeString()
                    : cs.WebformName;

                //Create webform
                _clonedWebformId = _service.Create(newWebform);
                worker.ReportProgress(25, true);
            }
            catch (Exception error)
            {
                _logger.LogInfo(error.Message);
                var percentage = 25;
                worker.ReportProgress(percentage, false);
            }


            return false;
        }
    }
}
