using System.Collections.Generic;
using System.Text;

namespace Poly.Validation.DataContainers
{
	public enum PolyMessageSeverity
	{
		None = 0,
		Error = 1,
		PerformanceWarning = 2,
		Warning = 3,
		Info = 4,
	}

	public enum PolyValidationUsecase : short
	{
		/** No usecase specified */
		None = 0,

		/** Triggered on user's demand */
		Manual,

		/** Saving a triggered the validation */
		Save,

		/** Submit dialog triggered the validation */
		PreSubmit,

		/** Triggered by c# */
		Script,
	}

	public struct PolyValidationIssue
	{
		public string message;
		public PolyMessageSeverity severity;

		public PolyValidationIssue(string message, PolyMessageSeverity severity)
		{
			this.message = message;
			this.severity = severity;
		}
	}

	public class PolyValidationContext
	{
		private readonly PolyValidationUsecase validationUsecase;
		private readonly List<PolyValidationIssue> issues = new();

		private int numWarnings = 0;
		private int numErrors = 0;

		public PolyValidationContext(PolyValidationUsecase validationUsecase)
		{
			this.validationUsecase = validationUsecase;
		}

		public PolyValidationUsecase GetValidationUsecase()
		{
			return validationUsecase;
		}

		public void AddWarning(string text)
		{
			issues.Add(new PolyValidationIssue(text, PolyMessageSeverity.Warning));
			++numWarnings;
		}

		public void AddError(string text)
		{
			issues.Add(new PolyValidationIssue(text, PolyMessageSeverity.Error));
			++numErrors;
		}

		public List<PolyValidationIssue> GetIssues()
		{
			return issues;
		}

		public int GetNumWarnings()
		{
			return numWarnings;
		}

		public int GetNumErrors()
		{
			return numErrors;
		}

		public void SplitIssues(List<string> warnings, List<string> errors)
		{
			foreach (var issue in issues)
			{
				if (issue.severity == PolyMessageSeverity.Warning)
				{
					warnings.Add(issue.message);
				}

				if (issue.severity == PolyMessageSeverity.Error)
				{
					errors.Add(issue.message);
				}
			}
		}

		public string GetAggregatedMessage()
		{
			var builder = new StringBuilder();
			foreach (var issue in issues)
			{
				if (issue.severity == PolyMessageSeverity.Warning)
				{
					builder.AppendLine($"- <color=yellow>[Warning]</color>{issue.message}");
				}

				if (issue.severity == PolyMessageSeverity.Error)
				{
					builder.AppendLine($"- <color=red>[Error]</color>{issue.message}");
				}
			}
			return builder.ToString();
		}
	}
}