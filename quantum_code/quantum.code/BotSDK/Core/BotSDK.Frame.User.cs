using Photon.Deterministic;

namespace Quantum
{
	public unsafe partial class Frame
	{
		// If you are interested, check BotSDKTimerSystem in order to see how the time counter logic is implemented
		internal FP BotSDKGameTime
		{
			get
			{
				// Try to get a game time value from a user implementation
				// If the value is not greater thane zero, either because there is no implementation
				// or because it calculates the time wrongly, it will use the default Bot SDK
				// time polling calculus
				FP gameTime = -FP._1;
				CalculateBotSDKGameTime(ref gameTime);
				if (gameTime >= FP._0)
				{
					return gameTime;
				}

				// We use division of integers in order to avoid accuracy issues with multiplications with DeltaTime
				return (FP)Global->BotSDKData.ElapsedTicks / SessionConfig.UpdateFPS;
			}
		}

		// Method meant to be used for user implementation of the time counter, if needed
		// Store the calculation result on the gameTime variable. The result has to be greater than zero
		partial void CalculateBotSDKGameTime(ref FP gameTime);
	}
}
