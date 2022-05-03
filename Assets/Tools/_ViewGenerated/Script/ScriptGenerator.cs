

namespace ViewGenerator
{
    
    [System.Serializable]
    public class ScriptGenerator
    {

        public string className = string.Empty;
        public string textContent = string.Empty;


        public override string ToString()
        {
            textContent = textContent.Replace("$ClassName", className);
            return textContent;
        }


    }
}

