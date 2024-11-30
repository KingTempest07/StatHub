using System.Linq;
using Godot;

namespace StatHub;

/// <summary>
/// A tag matcher decides whether or not a tag holder matches its set 
/// requirements for use with global modifiers.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/TagMatcher.png")]
public partial class TagMatcher : Resource
{
	/// <summary>
	/// The tag holder used to filter applicable tags.
	/// </summary>
	/// <remarks>
	/// By default, this is a whitelist.
	/// </remarks>
	[Export]
	public TagHolder TagFilter { get; private set; }

	/// <summary>
	/// If true, turns the tag filter into a blacklist rather than a whitelist.
	/// </summary>
	[Export]
	public bool InvertTagFilter { get; private set; }


	/// <summary>
	/// The amount of matches required to be considered matching; if less than 
	/// zero, will require all tags in the filter to be matched.
	/// </summary>
	/// <remarks>
	/// If <c>InvertTagFilter</c> is <c>true</c>, this will be flipped as well, 
	/// requiring the same amount of matches with the filter to *not* be 
	/// considered a match.
	/// </remarks>
	/// <value></value>
	[Export]
	public int RequiredTagMatches { get; private set; } = 1;


	/// <summary>
	/// Decides whether or not the tag holder matches this matcher’s criteria.
	/// </summary>
	/// <param name="tagHolder">The tag holder to check</param>
	/// <returns>Whether or not the tag holder matches</returns>
    public bool Matches(TagHolder tagHolder)
	{
		if (RequiredTagMatches == 0)
		{
			return !InvertTagFilter;
		}

		if (tagHolder == null)
		{
			return false;
		}

		if (tagHolder.Count() < RequiredTagMatches)
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

            __hasMetMatches = RequiredTagMatches switch
            {
                > 0 => __matchCount >= RequiredTagMatches
					|| RequiredTagMatches < 0
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
