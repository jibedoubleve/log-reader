using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Probel.LogReader.Colouration
{
    public class Colourator : IColourator
    {
        #region Fields

        private readonly IEnumerable<TextEditor> _editors;

        #endregion Fields

        #region Constructors

        public Colourator(params TextEditor[] editor)
        {
            _editors = editor;
        }

        #endregion Constructors

        #region Methods

        public void Set(string colouration)
        {
            var res = GetResourceName(colouration);

            if (string.IsNullOrEmpty(res)) { SetHighlighter(null); }
            else
            {
                using (var stream = GetResource(res))
                using (var reader = new XmlTextReader(stream))
                {
                    var hl = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    SetHighlighter(hl);
                }
            }
        }

        private Stream GetResource(string name) => Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

        private string GetResourceName(string type)
        {
            switch (type?.ToLower())
            {
                case "sql": return "Probel.LogReader.Colouration.Sql.xshd";
                default: return null;
            }
        }

        private void SetHighlighter(IHighlightingDefinition hl)
        {
            foreach (var editor in _editors)
            {
                editor.SyntaxHighlighting = hl;
            }
        }

        #endregion Methods
    }
}