using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace BLW.Lib.CoreUtility
{
  public  class PatternMatchingMapper
    {
        
        public List<String> masks = null;
        public Dictionary<String, String> _mapper;

        public PatternMatchingMapper()
        {
            _mapper = new Dictionary<String, String>();
            _mapper.Add("RUN_NUMBER", "RUN_NUMBER_UNDEFINED");
            _mapper.Add("CLIENT_NAME", "CLIENT_NAME_UNDEFINED");
            _mapper.Add("APP_NAME", "APP_NAME_UNDEFINED");
            _mapper.Add("FILENAME_EXT", "Input_Extension_UNDEFINED");
            _mapper.Add("FILE_NAME", "FileName_UNDEFINED");
            _mapper.Add("FILENAME_WITHOUT_EXT", "FileName_UNDEFINED");
            Console.WriteLine("Initilize Mapper.");
        }

        public void SetCurrentDateFormat()
        {
            DateTime currentDate = DateTime.Now;           
             List<String> matches = new List<string>();
             matches.Add( "dd{0}MM{0}yyyy");
             matches.Add("dd{0}MM{0}yy");
             matches.Add("dd{0}MM");
             matches.Add("MM{0}dd{0}yyyy");
             matches.Add("MM{0}dd{0}yy");
             matches.Add("MM{0}dd");
             matches.Add("yyyy{0}MM{0}dd");
             matches.Add("yy{0}MM{0}dd");
             matches.Add("yy{0}MM");
             List<String> sperators = new List<String>(){ "-","_","/"};
             foreach(var sperator in sperators){
                 foreach (var match in matches)
                 {
                     var token = String.Format(match, sperator);
                     _mapper.Add("NOW-" + token, currentDate.ToString(token));
                 }
             }
             
        }

        public void SetArrivalDateFormat(DateTime fileArivalDate)
        {
            DateTime arrivalDate = fileArivalDate;
            List<String> matches = new List<string>();
            matches.Add("dd{0}MM{0}yyyy");
            matches.Add("dd{0}MM{0}yy");
            matches.Add("dd{0}MM");
            matches.Add("MM{0}dd{0}yyyy");
            matches.Add("MM{0}dd{0}yy");
            matches.Add("MM{0}dd");
            matches.Add("yyyy{0}MM{0}dd");
            matches.Add("yy{0}MM{0}dd");
            matches.Add("yy{0}MM");
            List<String> sperators = new List<String>() { "-", "_", "/" };
            foreach (var sperator in sperators)
            {
                foreach (var match in matches)
                {
                    var token = String.Format(match, sperator);
                    _mapper.Add("ARRVL-" + token, arrivalDate.ToString(token));
                }
            }

        }

        public void SetFileFormat(string file)
        {
            var fileFullName = Path.GetFileName(file);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
            var fileExt = Path.GetExtension(file);
            SetMappingValue("FILE_NAME", fileFullName);
            SetMappingValue("FILENAME_WITHOUT_EXT", fileNameWithoutExt);
            SetMappingValue("FILENAME_EXT", fileExt);
            Console.WriteLine("File Details added to Mapper.");
        }

        public void SetClientAndAppDetails(String runNo)
        {          
           SetMappingValue("RUN_NUMBER", runNo);
           Console.WriteLine("Run number details added to Mapper.");
        }

        public void SetClientAndAppDetails(string clientName, string appName)
        {
            SetMappingValue("APP_NAME", appName.ToUpper());
            SetMappingValue("CLIENT_NAME", clientName.ToUpper());
            Console.WriteLine("Client details added to Mapper.");
        }
             
        public String GetMappingValue(String token)
        {
            return Convert.ToString(_mapper[token]);
        }

        public void SetMappingValue(String token, String value)
        {
            _mapper[token] = value;
        }

        public string EvaluateString(String Expresssion)
        {
            var tokens = _mapper.Select(x => x.Key).ToList<String>();
            foreach (var token in tokens)
            {
                var value = GetMappingValue(token);
                Expresssion = Expresssion.Replace("{{" + token + "}}", value);
                Expresssion = Expresssion.Replace("<" + token + ">", value);
            }
            return Expresssion;
        }      

    }     
}
