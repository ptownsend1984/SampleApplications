using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Google.Apis.Gmail.v1.Data;
using System.ComponentModel.DataAnnotations;

namespace GMailLabelCleanup.Core.Models
{
    public class LabelInfo
    {

        public string Id { get; set; }
        public string Name { get; set; }

        public LabelType LabelType { get; set; }
        public bool IsSystemType { get { return this.LabelType == LabelType.System; } }
        public bool IsUserType { get { return this.LabelType == LabelType.User; } }

        public LabelListVisibilityType LabelListVisibilityType { get; set; }
        public MessageListVisibilityType MessageListVisibilityType { get; set; }

    }

    public static class LabelInfoExtensions
    {
        public static Label ToLabel(this LabelInfo labelInfo)
        {
            System.Diagnostics.Debug.Assert(labelInfo != null);

            return new Label
            {
                Id = labelInfo.Id,
                Name = labelInfo.Name,
                Type = FromLabelType(labelInfo.LabelType),
                LabelListVisibility = FromLabelListVisibilityType(labelInfo.LabelListVisibilityType),
                MessageListVisibility = FromMessageListVisibilityType(labelInfo.MessageListVisibilityType)
            };
        }
        private static string FromLabelType(LabelType labelType)
        {
            switch (labelType)
            {
                case LabelType.System:
                    return "system";
                case LabelType.User:
                default:
                    return "user";
            }
        }
        private static string FromLabelListVisibilityType(LabelListVisibilityType labelListVisibilityType)
        {
            switch (labelListVisibilityType)
            {
                case LabelListVisibilityType.ShowIfUnread:
                    return "labelShowIfUnread";
                case LabelListVisibilityType.Hide:
                    return "labelHide";
                case LabelListVisibilityType.Show:
                default:
                    return "labelShow";
            }
        }
        private static string FromMessageListVisibilityType(MessageListVisibilityType messageListVisibilityType)
        {
            switch (messageListVisibilityType)
            {
                case MessageListVisibilityType.Hide:
                    return "hide";
                case MessageListVisibilityType.Show:
                default:
                    return "show";
            }
        }

        public static LabelInfo ToLabelInfo(this Label label)
        {
            System.Diagnostics.Debug.Assert(label != null);

            return new LabelInfo
            {
                Id = label.Id,
                Name = label.Name,
                LabelType = ToLabelType(label.Type),
                LabelListVisibilityType = ToLabelListVisibilityType(label.LabelListVisibility),
                MessageListVisibilityType = ToMessageListVisibilityType(label.MessageListVisibility)
            };
        }
        private static LabelType ToLabelType(string type)
        {
            switch (type)
            {
                case "system":
                    return LabelType.System;
                case "user":
                default:
                    return LabelType.User;
            }
        }
        private static LabelListVisibilityType ToLabelListVisibilityType(string labelListVisibility)
        {
            switch (labelListVisibility)
            {
                case "labelHide":
                    return LabelListVisibilityType.Hide;
                case "labelShowIfUnread":
                    return LabelListVisibilityType.ShowIfUnread;
                case "labelShow":
                default:
                    return LabelListVisibilityType.Show;
            }
        }
        private static MessageListVisibilityType ToMessageListVisibilityType(string messageListVisibility)
        {
            switch (messageListVisibility)
            {
                case "hide":
                    return MessageListVisibilityType.Hide;
                case "show":
                default:
                    return MessageListVisibilityType.Show;
            }
        }

    }

    public enum LabelType
    {
        System,
        User
    }

    public enum LabelListVisibilityType
    {
        ShowIfUnread,
        Show,
        Hide
    }

    public enum MessageListVisibilityType
    {
        Show,
        Hide
    }
}