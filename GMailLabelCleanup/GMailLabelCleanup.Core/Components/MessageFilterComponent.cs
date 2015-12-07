using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GMailLabelCleanup.Data.Documents.Filters;
using System.Xml.Linq;
using GMailLabelCleanup.Common.Extensions;
using GMailLabelCleanup.Data.Models.Filters;

namespace GMailLabelCleanup.Core.Components
{
    public class MessageFilterComponent
    {

        #region Variables & Properties

        private static readonly IReadOnlyDictionary<CriteriaPropertyType, string> _criteriaPropertyNames;
        public static IReadOnlyDictionary<CriteriaPropertyType, string> CriteriaPropertyNames { get { return _criteriaPropertyNames; } }

        private static readonly IReadOnlyDictionary<string, CriteriaPropertyType> _criteriaPropertyTypes;
        public static IReadOnlyDictionary<string, CriteriaPropertyType> CriteriaPropertyTypes { get { return _criteriaPropertyTypes; } }

        private static readonly IReadOnlyDictionary<ActionPropertyType, string> _actionPropertyNames;
        public static IReadOnlyDictionary<ActionPropertyType, string> ActionPropertyNames { get { return _actionPropertyNames; } }

        private static readonly IReadOnlyDictionary<string, ActionPropertyType> _actionPropertyTypes;
        public static IReadOnlyDictionary<string, ActionPropertyType> ActionPropertyTypes { get { return _actionPropertyTypes; } }

        private static readonly IReadOnlyDictionary<SmartLabelType, string> _smartLabelNames;
        public static IReadOnlyDictionary<SmartLabelType, string> SmartLabelNames { get { return _smartLabelNames; } }

        private static readonly IReadOnlyDictionary<string, SmartLabelType> _smartLabelTypes;
        public static IReadOnlyDictionary<string, SmartLabelType> SmartLabelTypes { get { return _smartLabelTypes; } }

        private static readonly IReadOnlyDictionary<SizeOperatorType, string> _sizeOperatorNames;
        public static IReadOnlyDictionary<SizeOperatorType, string> SizeOperatorNames { get { return _sizeOperatorNames; } }

        private static readonly IReadOnlyDictionary<string, SizeOperatorType> _sizeOperatorTypes;
        public static IReadOnlyDictionary<string, SizeOperatorType> SizeOperatorTypes { get { return _sizeOperatorTypes; } }

        private static readonly IReadOnlyDictionary<SizeUnitType, string> _sizeUnitNames;
        public static IReadOnlyDictionary<SizeUnitType, string> SizeUnitNames { get { return _sizeUnitNames; } }

        private static readonly IReadOnlyDictionary<string, SizeUnitType> _sizeUnitTypes;
        public static IReadOnlyDictionary<string, SizeUnitType> SizeUnitTypes { get { return _sizeUnitTypes; } }

        static MessageFilterComponent()
        {
            _criteriaPropertyNames = new Dictionary<CriteriaPropertyType, string>()
            {
                { CriteriaPropertyType.From, "from" },
                { CriteriaPropertyType.To, "to" },
                { CriteriaPropertyType.Subject, "subject" },
                { CriteriaPropertyType.HasTheWords, "hasTheWord" },
                { CriteriaPropertyType.DoesntHave, "doesNotHaveTheWord" },
                { CriteriaPropertyType.HasAttachment, "hasAttachment" },
                { CriteriaPropertyType.ExcludeChats, "excludeChats" },
                { CriteriaPropertyType.Size, "size" },
                { CriteriaPropertyType.SizeOperator, "sizeOperator" },
                { CriteriaPropertyType.SizeUnit, "sizeUnit" }
           }.AsReadOnly();
            _criteriaPropertyTypes = _criteriaPropertyNames.ToDictionary(o => o.Value, o => o.Key).AsReadOnly();

            _actionPropertyNames = new Dictionary<ActionPropertyType, string>()
            {
                { ActionPropertyType.ArchiveIt, "shouldArchive" },
                { ActionPropertyType.MarkAsRead, "shouldMarkAsRead" },
                { ActionPropertyType.StarIt, "shouldStar" },
                { ActionPropertyType.ApplyLabel, "label" },
                { ActionPropertyType.ForwardIt, "forwardTo" },
                { ActionPropertyType.DeleteIt, "shouldTrash" },
                { ActionPropertyType.NeverSendToSpam, "shouldNeverSpam" },
                { ActionPropertyType.AlwaysImportant, "shouldAlwaysMarkAsImportant" },
                { ActionPropertyType.NeverImportant, "shouldNeverMarkAsImportant" },
                { ActionPropertyType.SmartLabel, "smartLabelToApply" }
            }.AsReadOnly();
            _actionPropertyTypes = _actionPropertyNames.ToDictionary(o => o.Value, o => o.Key).AsReadOnly();

            _smartLabelNames = new Dictionary<SmartLabelType, string>()
            {
                { SmartLabelType.Personal, "^smartlabel_personal" },
                { SmartLabelType.Social, "^smartlabel_social" },
                { SmartLabelType.Promotions, "^smartlabel_promo" },
                { SmartLabelType.Notification, "^smartlabel_notification" },
                { SmartLabelType.Group, "^smartlabel_group" }
            }.AsReadOnly();
            _smartLabelTypes = _smartLabelNames.ToDictionary(o => o.Value, o => o.Key).AsReadOnly();

            _sizeOperatorNames = new Dictionary<SizeOperatorType, string>()
            {
                { SizeOperatorType.GreaterThan, "s_sl" },
                { SizeOperatorType.LessThan, "s_ss" }
            }.AsReadOnly();
            _sizeOperatorTypes = _sizeOperatorNames.ToDictionary(o => o.Value, o => o.Key).AsReadOnly();

            _sizeUnitNames = new Dictionary<SizeUnitType, string>()
            {
                { SizeUnitType.MB, "s_smb" },
                { SizeUnitType.KB, "s_skb" },
                { SizeUnitType.Byte, "s_sb" }
            }.AsReadOnly();
            _sizeUnitTypes = _sizeUnitNames.ToDictionary(o => o.Value, o => o.Key).AsReadOnly();
        }

        #endregion

        #region Constructor

        public MessageFilterComponent()
        {

        }

        #endregion

        #region Methods

        public string GetFilterIdPart(string idTag)
        {
            if (string.IsNullOrEmpty(idTag))
                return idTag;

            var match = System.Text.RegularExpressions.Regex.Match(idTag, @"filter:(\d+)");
            return match.Success && match.Groups.Count > 1 ? match.Groups[1].Value : idTag;
        }

        public IEnumerable<MessageFilterProperty> GetCriteriaProperties(IEnumerable<MessageFilterProperty> properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            return properties.Where(o => CriteriaPropertyTypes.ContainsKey(o.Name));
        }
        public IEnumerable<MessageFilterProperty> GetActionProperties(IEnumerable<MessageFilterProperty> properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            return properties.Where(o => ActionPropertyTypes.ContainsKey(o.Name));
        }

        public bool GetIsBooleanProperty(string name)
        {
            CriteriaPropertyType criteriaType;
            ActionPropertyType actionType;
            if (CriteriaPropertyTypes.TryGetValue(name, out criteriaType))
                return GetIsBooleanProperty(criteriaType);
            else if (ActionPropertyTypes.TryGetValue(name, out actionType))
                return GetIsBooleanProperty(actionType);
            else
                return false;
        }
        public bool GetIsBooleanProperty(CriteriaPropertyType type)
        {
            switch (type)
            {
                case CriteriaPropertyType.HasAttachment:
                    return true;
                default:
                    return false;
            }
        }
        public bool GetIsBooleanProperty(ActionPropertyType type)
        {
            switch (type)
            {
                case ActionPropertyType.ArchiveIt:
                case ActionPropertyType.MarkAsRead:
                case ActionPropertyType.StarIt:
                case ActionPropertyType.DeleteIt:
                case ActionPropertyType.NeverSendToSpam:
                case ActionPropertyType.AlwaysImportant:
                case ActionPropertyType.NeverImportant:
                    return true;
                default:
                    return false;
            }
        }

        public string GetDisplayText(MessageFilterProperty property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            CriteriaPropertyType criteriaType;
            ActionPropertyType actionType;

            var name = property.Name;
            if (CriteriaPropertyTypes.TryGetValue(name, out criteriaType))
            {
                return GetCriteriaDisplayText(criteriaType, name, property.Value);
            }
            else if (ActionPropertyTypes.TryGetValue(name, out actionType))
            {
                return GetActionDisplayText(actionType, name, property.Value);
            }
            else
            {
                return GetDefaultDisplayText(name, property.Value);
            }
        }
        private string GetCriteriaDisplayText(CriteriaPropertyType type, string name, string value)
        {
            switch (type)
            {
                case CriteriaPropertyType.SizeOperator:
                    SizeOperatorType sizeOperator;
                    if (SizeOperatorTypes.TryGetValue(value, out sizeOperator))
                    {
                        switch (sizeOperator)
                        {
                            case SizeOperatorType.GreaterThan:
                                return "Size greater than";
                            case SizeOperatorType.LessThan:
                                return "Size less than";
                        }
                    }
                    break;
                case CriteriaPropertyType.SizeUnit:
                    SizeUnitType sizeUnit;
                    if (SizeUnitTypes.TryGetValue(value, out sizeUnit))
                    {
                        switch (sizeUnit)
                        {
                            case SizeUnitType.MB:
                                return "Size in MB";
                            case SizeUnitType.KB:
                                return "Size in KB";
                            case SizeUnitType.Byte:
                                return "Size in bytes";
                        }
                    }
                    break;
            }
            return GetDefaultDisplayText(name, value);
        }
        private string GetActionDisplayText(ActionPropertyType type, string name, string value)
        {
            switch (type)
            {
                case ActionPropertyType.SmartLabel:
                    SmartLabelType smartLabel;
                    if (SmartLabelTypes.TryGetValue(value, out smartLabel))
                    {
                        switch (smartLabel)
                        {
                            case SmartLabelType.Personal:
                                return "Categorize as personal";
                            case SmartLabelType.Social:
                                return "Categorize as social";
                            case SmartLabelType.Promotions:
                                return "Categorize as promotions";
                            case SmartLabelType.Notification:
                                return "Categorize as updates";
                            case SmartLabelType.Group:
                                return "Categorize as forums";
                        }
                    }
                    break;
            }
            return GetDefaultDisplayText(name, value);
        }
        private string GetDefaultDisplayText(string name, string value)
        {
            return string.Format("{0}:({1})", name, value);
        }

        public string CreateDescription(MessageFilterEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            var criteria = GetCriteriaProperties(entry.Properties).OrderBy(o => o.Name).Select(GetDisplayText);
            var actions = GetActionProperties(entry.Properties).OrderBy(o => o.Name).Select(GetDisplayText);
            var description = string.Join(Environment.NewLine, criteria.Concat(actions));
            if (description.Length > 497)
                description = description.Substring(0, 497) + "...";

            return description;
        }

        public MessageFilter FromXml(XDocument document, string userId)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            var ns = document.Root.GetDefaultNamespace();
            var appsNs = document.Root.GetNamespaceOfPrefix("apps");

            var entries = document.Root.Elements(ns + "entry");
            var filterEntries = entries
                .Where(o => o.Element(ns + "id") != null)
                .Select(o => new MessageFilterEntry
                {
                    IdTag = o.Element(ns + "id").Value,
                    Properties = o.Elements(appsNs + "property")
                        .Where(p => p.Attribute("name") != null && p.Attribute("value") != null)
                        .Select(p => new MessageFilterProperty
                        {
                            Name = p.Attribute("name").Value,
                            Value = p.Attribute("value").Value
                        }).ToList()
                }).ToList();

            return new MessageFilter
            {
                UserId = userId,
                DateCreatedUtc = DateTime.UtcNow,
                Entries = filterEntries
            };
        }
        public XDocument ToXml(IEnumerable<Filter> filters)
        {
            if (filters == null)
                throw new ArgumentNullException("filters");

            var ns = (XNamespace)"http://www.w3.org/2005/Atom";
            var appsNs = (XNamespace)"http://schemas.google.com/apps/2006";

            var entries = filters.Select(o =>
                new XElement(ns + "entry",
                    o.FilterProperties.Where(p => p.IsIncluded).Select(p =>
                        new XElement(appsNs + "property", new XAttribute("name", p.Name.IfNotEmpty()), new XAttribute("value", p.Value.IfNotEmpty()))
                        )
                    ));

            var root = new XElement(ns + "feed", new XAttribute(XNamespace.Xmlns + "apps", appsNs), entries);

            return new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), root);
        }

        #endregion

    }

    public enum CriteriaPropertyType
    {
        Unknown = 0,
        From = 1 << 1,
        To = 1 << 2,
        Subject = 1 << 3,
        HasTheWords = 1 << 4,
        DoesntHave = 1 << 5,
        HasAttachment = 1 << 6,
        ExcludeChats = 1 << 7,
        Size = 1 << 8,
        SizeOperator = 1 << 9,
        SizeUnit = 1 << 10
    }
    public enum ActionPropertyType
    {
        Unknown = 0,
        ArchiveIt = 1 << 16,
        MarkAsRead = 1 << 17,
        StarIt = 1 << 18,
        ApplyLabel = 1 << 19,
        ForwardIt = 1 << 20,
        DeleteIt = 1 << 21,
        NeverSendToSpam = 1 << 22,
        AlwaysImportant = 1 << 23,
        NeverImportant = 1 << 24,
        SmartLabel = 1 << 25
    }
    public enum SmartLabelType
    {
        Unknown,
        Personal,
        Social,
        Promotions,
        Notification,
        Group
    }
    public enum SizeOperatorType
    {
        Unknown,
        GreaterThan,
        LessThan
    }
    public enum SizeUnitType
    {
        Unknown,
        MB,
        KB,
        Byte
    }

}