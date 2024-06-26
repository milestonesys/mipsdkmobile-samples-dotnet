using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace BookmarkSample
{
    public class BookmarkViewEntry
    {
        public Guid Id { get; set; }
        public string CameraName { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public DateTime TimeBegin { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime Time { get; set; }
        public string User { get; set; }

        public BookmarkViewEntry(SerializableItem bookmark)
        {
            Id = bookmark.Id;
            CameraName = bookmark.Children[0].Name;
            Description = bookmark.Properties[CommunicationCommands.Description];
            Name = bookmark.Name;
            Reference = bookmark.Properties[CommunicationCommands.Reference];
            TimeBegin = ParseEpoch(bookmark.Properties[CommunicationCommands.StartTime]);
            TimeEnd = ParseEpoch(bookmark.Properties[CommunicationCommands.EndTime]);
            Time = ParseEpoch(bookmark.Properties[CommunicationCommands.Time]);
            User = bookmark.Properties[CommunicationCommands.UserName];
        }

        private DateTime ParseEpoch(string epoch)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(epoch)).DateTime;
        }
    }
}
