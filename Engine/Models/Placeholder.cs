using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    /// <summary>
    /// A substitute for a MusicXML property we don't want or need to support to make our program simpler and easier to maintain,
    /// but needs to exist in the score for correct timing.
    /// 
    /// For example, delayed inverted turns are too rare to program for, but they are still a measure element that affects
    /// the duration and positioning of other notes.
    /// </summary>
    public class Placeholder : SpanningMeasureElement
    {
    }
}
