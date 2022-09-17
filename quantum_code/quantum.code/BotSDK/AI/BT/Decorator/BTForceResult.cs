﻿using System;

namespace Quantum
{
	[Serializable]
	public unsafe partial class BTForceResult : BTDecorator
	{
		public BTStatus Result;

		protected override BTStatus OnUpdate(BTParams btParams)
		{
			if (_childInstance != null)
				_childInstance.RunUpdate(btParams);

			return Result;
		}

		public override Boolean DryRun(BTParams btParams)
		{
			return true;
		}
	}
}
