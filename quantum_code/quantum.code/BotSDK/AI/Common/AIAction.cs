using System;
using Photon.Deterministic;

namespace Quantum
{
	public abstract unsafe partial class AIAction
	{
		public string Label;
		public const int NEXT_ACTION_DEFAULT = -1;

		public abstract void Update(Frame frame, EntityRef entity);
		public virtual int NextAction(Frame frame, EntityRef entity) { return NEXT_ACTION_DEFAULT; }
	}
}
