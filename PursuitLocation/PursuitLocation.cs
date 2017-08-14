using System;
using LSPD_First_Response.Mod.API;
using Rage;

namespace PursuitLocation
{
	public class Main : Plugin
	{
		public override void Initialize()
		{
			Game.DisplayNotification("PursuitLocation ~g~ loaded");
			
			Controller controller = new Controller();
			
			GameFiber.StartNew(delegate {
			                   	
				GameFiber.Sleep(1000);
				while (true) {
					GameFiber.Yield();
					GameFiber.Sleep(1000);
					
					controller.process();
				}							                   	
			}, "PursuitLocationFiber");
		}
		public override void Finally()
		{
			
		}
	}
}