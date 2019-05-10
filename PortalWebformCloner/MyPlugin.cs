using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace KT.PortalWebformCloner
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Portal Webform Cloner"),
        ExportMetadata("Description", "Portal webform Clone tool to create Copy of webform with different Modes"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "/9j/4AAQSkZJRgABAQEAAQABAAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCAAgACADASIAAhEBAxEB/8QAGQAAAgMBAAAAAAAAAAAAAAAABggBAgMK/8QAKxAAAgMAAQMDAgUFAAAAAAAAAgMBBAUGAAcREiExCFETFBVTYRYjMpPS/8QAGAEAAwEBAAAAAAAAAAAAAAAAAgMFBgj/xAAfEQADAQACAwEBAQAAAAAAAAABAgMEBQYREhMUIiP/2gAMAwEAAhEDEQA/AOb/AK1Qh1lya1ZLrFmy5NatWrqY+xZs2GCmvWrV0ibrFmw4wTXrpBjnuMFKA2EIzl0/H0b0ez54vcXTs6PI5+onNpV7XbLKHMzGYi8Krdqu5Lp4uhN4NpWwGLGr/V+xi0mc64JxBJ8s7PJ0e4lahXDlTuvaF6d13Zzzcbu5Q56ZM6ZcMaV9abtUcc9e6sp1OHicTWGrluRadFw4JX0fKzokaaDq/AnsvNZuIG3LgFk0Va+qiJ7JlhTQ2fJN3T9XIahP8/H41dG1a6Sj7zVmoiFMWxLGpas1OS1qHKaBral6TJTkuUyBYpyWganJYIsU0CWwRMSGKdOl9YNHgaNzHuZscYocz0KVS7ax+F/krqk4dr8dtdnOdKnoHmWr0Vv0tvbTezaQcx5xwa23kffSvjd0iu03Jb0/qXYl7X17judTFo48bpF2zaB5KujFGeFgFTXjqR9MmyYE9MGSgVGLIquxcM3X+Y2cS+qOw5KBfvDyAwZQwWsiS+fQgPpfPT+41DJ5YAM2qUPsFIV0OsHA+qQQpjjgYmIkvQsSL0xJDEz48RMx9+jHhebI8lzrOls73ClZpu1qnJ8rH1rujjbuVWbo8bsVF5kL0athm9Xz66tSmQ2cc3DqK9R1IWYVBEM+RIhn48iUjPj7eYmJ8fx1PrZ+4z/Yf/XVrXCumGjOtvgt41j9UlKtZfVChpNdCWzM6e3si6M14lgBWVU9kaZmtOFo2aX2MbTr82pSc6CbKwR2i0rqrePDNG0qgH/Ok38OCjkuZP63oNynb+9Usmq+7Y1Mq1X0L+npVkaO2+4EgbGWI2LV5Lbjyl+ixRaLJmbYzI05D65QFhLUGQCwQcs1FKzjyBwJwMyBx7iUR6Sj4meq/iM/db7z5n+4fz9/8vn2j3/jqszM/MzM/HkpmZ8R8R5mZ9o+3RZpVhKMXqLCUpy+jTVK0M0VPo/z9Ihn8FnEozmCfE5ooCgbUnWlaLMy+lGcIrlkQMxb0X39qFV8+ql6O5ABd3byx//Z"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "/9j/4AAQSkZJRgABAQEAAQABAAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCABQAFADASIAAhEBAxEB/8QAHAABAAICAwEAAAAAAAAAAAAAAAUGBwkDBAgK/8QAKBAAAQQCAQQBBAMBAAAAAAAAAwECBAUABgcIERITFBUWISIjMTPw/8QAGgEBAAMBAQEAAAAAAAAAAAAAAAQGCAIFB//EACwRAAICAQQBAwIGAwEAAAAAAAECAwQFAAYREiEHE2EUIhUxMlFScQglQWL/2gAMAwEAAhEDEQA/APm/xjGZ/wBNMYxjTTGMY00xjGNNMYxjTTGMY00xjNiHQp0WXnUdbQuTD3+qQtA455O1qDtuu3lbaWths8OCKu2ewrIsUAEqPh2EIgas6Wktg3tkyHPAQQ0Yeob63ztz052xkt27qvpjsRjY17zOkrmazM3tU6cSQxSu09yw0deH7OokkUuyoGYWLam1s1vPO0dvYGo9zI3n4WNSiiOFOGsWZDI8a+1Wi7TScN2KKQgZiAaD05dLkfcqIHPPOEkuodKNM/Y4m3b3VbDAFsS21a9lJVVNLRw4d7fnNN26wq65xR0jxPD8tRmZ4ITKZzD057DrNJf828e6/sFh0u2W1kruNeR9gsKZthdUEyzPT0c6yqFPC2CMs6yhzIQZU3X65CqMBCAB8gfluJ6veOrrhbeNa57vCUt10ZaLL0yn2LphoxMpqJ9jZuu4CXAdINEg8b3zvvC1q9jKloUM5x4/vVquhhfmPtt5XuKLU9f6waSKHkPof2muqePw9ItlU09NB02yY2XrgrBadlJsXH6Bpd31wlwCwCYZzLdCRrmPciLkDAeve9Nw38LvzAtSy+E3XMmIx2Be5PV23BbSV8hX9M68kqJJF613a0hlfcuTnqbGkxscNOKv73Wy+kMv6SbYxFPJ7Uyos43J7ejbI3sslWGznJK7xR1Jt7TpG7q/plWnT20wlGOxuhbryWXm9s+wuhj/AL8fnGeo+cuHNjhalr/U+Gq0zVuL+oLdtxn8f6Rr90yZa6hBdYWk+LST62PVV8CNFgxIpYwvphCjiuEEEmLAccAc8uZtPb+fx+5MamRx80UipPaoXoYporBx+Wx070sti5poS0L2cZkIbFKwYmeIzQOY3dCrHMOZxFvCXnpXI5ELRQW6srxvELmPuRLZx9+KOULKsN2pJFZhWVUkCSASIjhlDGMZ7evL0xjGNNMy1w9zPvPC+461tGqbFs0KDS7XQ7Lba1U7Pb0dRtA6eYAp6y5iwzLAlhsq9h6oppsCb64sl7fUQaepcS4yBlMXj81j7eKytOC/j70Ela3UsxrJDNDKhR1ZWHg9WPV14dG4ZGVgCJlC/dxdyvkMfZmp3KkqT17EDlJI5I2DKQR+Y5A7K3KOOVdWUkHeVbbtE6l5us9ZepTJm7X+hSoaWvQRY7DA2896DUJ0/W5+00dZHO8hBjqdjj7k2UfjWzN7a6QqFUfiQXWg8v8ABPBc3c+sGkszVnJ27QW6jsfQue9pNYHpG0T7moqNkJaVUelFNEOpbrTtrGkrT4bmEtnFcVSyGLmn7i7k7deGt5peSeO7MVJuFA2xFV2hqyvthhHa1smqsAkhWUeREkDkQJZwqwo1VnkhRqwjGuSL3rdNn5H3HZd+3Kf9V2nb7eVe39k2JGgsm2Uzw95hw4Qgw4o1QY2DjxhDCJjGtY3+1XMUf+NyHJnBTZYp6cwYyHFU6lO1PWy021fxL6iX0zyNP2Hp2MDGscFyDe8VyHf72GehLk/pFWRvujeth+hGVixqtvSW9JetT2YYZ8emd+hFePe9KwXWzDlJS8tabaz1n2msSR2o6nvsUElylvL+SuRd53xasVADcdw2Hawa5ElPlV9D9wWJZ5K+C70xROYJSIwhxQ4nyXt9rwjVUY2g4xmoaFGrjKVTHUYvYp0K0FOrD2eT2q9aJYYUMkrPLIVjRQZJXeRyOzuzEk/B7duxetWLtqT3bNueWzPJ1Ve80ztJIwRAqIGdmISNVjQHqiqoADGMZL1H0xjJOqkVseQR1rAJYRSB9XrFIWMUL3FEqyBETv8AyMC0zB/hUR5GqqK1HIvEjFEZ1RpSo5Eade7fC92Ref7YD510ih3VS6xhjwXfnqvy3UM3H9An41GYy1pZakjERNXmq9Xv8nP2KQrUGnr9frYkX/VfB6lcX2CcpFQYmNYxE6z5+up8hQ0EhHEiIKP8m1MYceUr5SPlvY1rXFVoSRUCFxFYw0ZSr3aRWLFW3MT5x9xfI/U1P9wOfstvwPJJPBPCkgMeAZBrxAji7VYH+ItcjwTxw1ZeT+Q48AMQpP8A0+nuIa3p/teENmi71f6NrfMf3ltBtCsNniSZkIteLjVHU0DdUj+RImpTtn+oBpZ4QmWHuy0B7sRNYZNG+j8k3XG0viPiH7W13jaBvdvH3cvJbdbhyR7BQSK3cmJo4ZBHzDDEyz1DzHORrpaT07yZhA2TWEzEx7bUila8Oqy4ov6UI9glP7fjur0I4KKr/NVREcjmerwav7DVTcbrXV0UCB1cyMZLHIKsm3kSikAzyQkNvdohIIzXeTnOGr/Nrf2REcr6PBtaZM1+MyXt22Ac42djxti/Q+grlsbkMe2LCw2FdsV3vQZFacks6R5DH1Zo2RTNFLbJtwRvixi46e3YSMSuIe9DUtC3OFu0rgvs0sJVcgVrS0WsqkTPStzxOrMsUkdUxls+pai3z7axPIvYKiV+wSGD79vKShWMjq9W+aqyKgysVg2DcdSk8/LjbZauyQAqa1KeBjzrIimuSkbIG+MgwsaVoRuCQMnvJ942tcrf4fX+vm67/Vzef9beHC9vuNIc/Z2Cji4fuP6RzwvbwWA86qZrxD8r1U+QOALXgFuvY81QOB+o8Enr5APIBq+MkbORXypDSVtetbHQLRrHU6yFcRpCqplI5rV8njcJjk/KeQ1c3xa5GMjslxsXRXaN4mYAmN+hdD/FijOnI/8ALMPnUd1COyhlcKeA69urD9x2VW4PyoPxpjGM71zpjGMaaYxjGmmMYxppjGMaa//Z"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public MyPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}