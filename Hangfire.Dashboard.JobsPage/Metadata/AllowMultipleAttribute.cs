﻿using System;

namespace Hangfire.Dashboard.JobsPage.Metadata
{
	/// <summary>
	/// Indicate that this method can be sheduled multiple times as a recurring job. (for example with different parameters)
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class AllowMultipleAttribute : Attribute
	{
		/// <summary>
		/// If set to True, it should be possible to shedule the method multiple times as a recurring job
		/// </summary>
		public bool AllowMultiple { get; set; }

		public AllowMultipleAttribute()
		{
			AllowMultiple = true;
		}
	}
}
