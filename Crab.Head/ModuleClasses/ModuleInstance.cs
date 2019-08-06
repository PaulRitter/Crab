using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using System.Xml;
using System.IO;

namespace Crab
{
    public abstract class ModuleInstance //if you want instances for your modules
    {
        //name of datafile (without ending)
        private readonly string datafile = ""; //leave empty to disable saving data

        public async Task<ModuleInstanceResult> mainAsync(){
            string datafilePath = "data/"+datafile+".xml";

            if(datafile.Length > 0){
                if(File.Exists(datafilePath)){
                    using (XmlReader reader = XmlReader.Create(datafilePath))
                    {
                        await loadData(reader);
                    } 
                }else{
                    File.Create(datafilePath);
                }
            }

            await startAsync();

            exitEvent = new AsyncManualResetEvent();
            await exitEvent.WaitAsync();

            shutdown();
            
            if(datafile.Length > 0){
                if(File.Exists(datafilePath)){
                    using (XmlWriter writer = XmlWriter.Create(datafilePath))
                    {
                        await saveData(writer);
                    }
                }else{
                    File.Create(datafilePath);
                }
            }
            
            return exitCode;
        }


        public async Task loadData(XmlReader reader){
            await Task.CompletedTask;
        }

        //https://docs.microsoft.com/de-de/dotnet/api/system.xml.xmlwriter?view=netframework-4.8
        public async Task saveData(XmlWriter writer){
            await Task.CompletedTask;
        }

        public abstract Task startAsync();

        public abstract void shutdown(); //this should not be async because of obvious reasons

        private ModuleInstanceResult exitCode = ModuleInstanceResult.NONE; 
        private AsyncManualResetEvent exitEvent;
        public void exit(ModuleInstanceResult code){
            exitCode = code;
            exitEvent.Set();
        }
    }
}