using System;
using System.Collections.Generic;
using Godot;

namespace StatHub;

/// <summary>
/// DOC
/// </summary>
[GlobalClass]
public sealed partial class FigureExtremum : Node
{
	/// <summary>
	/// DOC
	/// </summary>
    [Export]
	public Stat ExtremumValue { get; private set; }


	public enum FitOptions 
	{
		INSTANT = 0,
		LINEAR = 1,
		DISABLED = 255
	}
	[Export]
	public FitOptions FitOption { get; private set; }

	[Export]
	public float LinearFitRate { get; private set; } = 1;

	private readonly Dictionary<Figure, float> m_CurrentlyFitting = new();


	public enum KeepProgressOptions 
	{
		UNCHANGED = 0,
		PERCENTAGE = 1,
		DISTANCE = 2,
	}
	[Export]
	public KeepProgressOptions KeepProgressOption { get; private set; }


	private readonly Dictionary<Figure, int> m_RegisteredFigures = new();

	public void Register(Figure figure, int inwardDirection)
	{
		inwardDirection = inwardDirection < 0 
			? -1
			: inwardDirection > 0
				? 1
				: throw new ArgumentException("The input inwardDirection must not be zero!");
		m_RegisteredFigures.Add(figure, inwardDirection);
	}


	public void Update(Figure figure, double delta = 0) 
	{
		int __inwardDirection = m_RegisteredFigures[figure];

		bool __inBounds = __inwardDirection == 1 
					   && figure >= ExtremumValue
					   || figure <= ExtremumValue;

        if (!__inBounds)
		{
            __Fit();
			return;
        }

		__KeepProgress();
		m_CurrentlyFitting.Remove(figure);


        void __Fit() 
		{ 
			switch (FitOption)
			{
				case FitOptions.DISABLED:
				{
					return;
				}
				case FitOptions.INSTANT:
				{// TODO asdasdadsadasdasdasdadasdasdaasdhhrt6rdgfdbftyhtytbytrhedrdsfv5trhrtbbxvsfwefwfebfntfrytrrververvrethtyrtghymtyetrgdefverdfgrthtrfjntfgntfrhrtyretfgbfgbfgbfhgthf
					figure.SetBypassed(ExtremumValue);
					break;
				}
				case FitOptions.LINEAR:
				{
					if (m_CurrentlyFitting.ContainsKey(figure))
					{
						m_CurrentlyFitting[figure] += (float) delta;
						figure.AddBypassed(
							LinearFitRate * (float) delta * __inwardDirection
						);
						break;
					}
					m_CurrentlyFitting.Add(figure, 0);
					break;
				}
			}
		}

		void __KeepProgress() 
		{
			switch (KeepProgressOption)
			{
				case KeepProgressOptions.UNCHANGED:
				{
					return;
				}
				case KeepProgressOptions.PERCENTAGE:
				{
					float __range = figure.Maximum - figure.Minimum;
					float __percentage = 
				}
			}
		}
	}
}