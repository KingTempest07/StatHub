using System.Linq;
using Godot;

namespace StatHub;

/// <summary>
/// DOC
/// </summary>
[GlobalClass]
public partial class TagMatcher : Resource
{
	/// <summary>
	/// DOC
	/// </summary>
	/// <value></value>
	[Export]
	public TagHolder TagFilter { get; private set; }

	/// <summary>
	/// If true, turns the tag filter into a blacklist rather than a whitelist
	/// </summary>
	/// <value></value>
	[Export]
	public bool InvertTagFilter { get; private set; }


	/// <summary>
	/// The amount of matches required to be considered matching; if less than 
	/// zero, will require all tags in the filter to be matched
	/// </summary>
	/// <remarks>
	/// If <c>InvertTagFilter</c> is <c>true</c>, then this will be flipped as 
	/// well, requiring the same amount of matches with the filter to *not* be 
	/// considered a match.
	/// </remarks>
	/// <value></value>
	[Export]
	public int TagMatchesRequired { get; private set; } = 1;


	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="tags"></param>
	/// <returns></returns>
    public bool Matches(TagHolder tagHolder)
	{
		if (TagMatchesRequired == 0)
		{
			return !InvertTagFilter;
		}

		if (tagHolder == null)
		{
			return false;
		}

		if (tagHolder.Count() < TagMatchesRequired)
		{
			return InvertTagFilter;
		}

		int __matchCount = 0;
		bool __hasMetMatches = false;

		foreach (Tag __tag in tagHolder)
		{
			if (!TagFilter.Contains(__tag))
			{
				continue;
			}
			__matchCount++;

            __hasMetMatches = TagMatchesRequired switch
            {
                > 0 => __matchCount >= TagMatchesRequired
					|| TagMatchesRequired < 0
					&& __matchCount == TagFilter.Count(),
                
				_ => __matchCount == TagFilter.Count(),
            };

            if (__hasMetMatches)
			{
				break;
			}
		}

        return __hasMetMatches
			? !InvertTagFilter 
			: InvertTagFilter;
    }
}
