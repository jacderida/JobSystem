using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.Community.Tasks
{
	public class VersionFormat : Task
	{
		public VersionFormat()
		{
			MajorVersionPadding = 2;
			MinorPadding = 2;
			RevisionPadding = 2;
			BuildPadding = 3;
			Build = -1;
			Revision = -1;
		}

		#region Public Properties

		[Required]
		public int Major { get; set; }
		public int MajorVersionPadding { get; set; }

		[Required]
		public int Minor { get; set; }
		public int MinorPadding { get; set; }

		
		public int Revision { get; set; }
		public int RevisionPadding { get; set; }

		public int Build { get; set; }
		public int BuildPadding { get; set; }

		[Output]
		public string FormattedBuildNumber { get; private set; }

		#endregion

		#region ITask Methods

		public override bool Execute()
		{
			var majorVersion = Major.ToString().PadLeft(MajorVersionPadding, '0');
			var minorVersion = Minor.ToString().PadLeft(MinorPadding, '0');
			var buildVersion = Build.ToString().PadLeft(BuildPadding, '0');
			
			if (Revision < 0)
			{
				if(Build > -1)
					FormattedBuildNumber = string.Format("{0}.{1}.{2}", majorVersion, minorVersion, buildVersion);
				else
				{
					FormattedBuildNumber = string.Format("{0}.{1}", majorVersion, minorVersion);
				}
			}
			else
			{
				var revision = Revision.ToString().PadLeft(RevisionPadding, '0');
				FormattedBuildNumber = string.Format("{0}.{1}.{2}.{3}", majorVersion, minorVersion,   buildVersion, revision);
			}
			return true;
		}

		#endregion
	}
}
