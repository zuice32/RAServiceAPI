using Core.Settings;

namespace Core.Logging
{
    /// <summary>
    ///     Filter log entries using a 'white list' of values to compare against the entry source.
    ///     Entries from sources that do not match any values in the 'white list' are not logged.
    /// </summary>
    public class WhiteSourceLogFilter : SourceLogFilterBase
    {
        public WhiteSourceLogFilter(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
            IsWhiteFilter = true;
        }

        public override string ThisClassName
        {
            get { return "Core.Logging.WhiteSourceLogFilter"; }
        }

        protected override string GetDefaultTokens()
        {
            string topLevelNamespaces = "System Microsoft";

            topLevelNamespaces += " " + "Core";

            topLevelNamespaces += " " + "(unknown source)";

            return topLevelNamespaces;
        }
    }
}