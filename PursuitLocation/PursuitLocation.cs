using System;
using LSPD_First_Response.Mod.API;
using Rage;

namespace PursuitLocation
{
	public class Main : Plugin
	{
		public override void Initialize()
		{
			Game.DisplayNotification("PursuitLocation ~g~loaded");
			
			Controller controller = new Controller();
			
			GameFiber.StartNew(delegate {
			                   	
				GameFiber.Sleep(1000);
				while (true) {
					GameFiber.Yield();
					GameFiber.Sleep(1000);
					
					try {
						controller.process();
					} catch (Exception) {
						Game.DisplayNotification("PursuitLocation occured an ~r~error");
						return;
					} 					
					
				}							                   	
			}, "PursuitLocationFiber");
		}
		public override void Finally()
		{
			
		}
	}
}