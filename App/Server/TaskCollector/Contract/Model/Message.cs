using System;

namespace TaskCollector.Contract.Model
{
    public class Message : Entity
    {
		public int Level { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string FeedbackContact { get; set; }
		public string AddFileds { get; set; }
		public Guid ClientId { get; set; }
		public DateTimeOffset CreatedDate { get; set; }

	}

	public class MessageHistory : EntityHistory
	{
		public int Level { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string FeedbackContact { get; set; }
		public string AddFileds { get; set; }
		public Guid ClientId { get; set; }
		public DateTimeOffset CreatedDate { get; set; }

	}

	public enum MessageLevelEnum
	{ 
	    Issue = 0,
		Warning = 1,
		Error = 10
	}

}
