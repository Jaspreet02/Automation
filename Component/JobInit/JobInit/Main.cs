using DbHander;
using System;

namespace JobInit
{
    class Main
    {
        public int Run(string runNo, string fileDownloadLocation)
        {
            try
            {
                IRunDetailsRepository repository = new RunDetailsRepository();
                RunDetail detail = repository.GetRunDetailByRunNumber(runNo);
                PreProcessor objPreProcessor = new PreProcessor();
                //Make entry for application components for each run number
                objPreProcessor.ProcessRunNumber(detail, fileDownloadLocation);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in preprocessing" + ex);
            }
            return 0;
        }
    }
}
