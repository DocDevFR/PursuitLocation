using System;
using LSPD_First_Response.Mod.API;
using Rage;

namespace PursuitLocation
{
	public class Controller
	{
		private LHandle pursuit;
		private bool isOnPursuit;
		private String lastStreetName;
		
		public Controller()
		{
			pursuit = null;
			isOnPursuit = false;
			lastStreetName = "";
		}
		
		public void process()
		{
			if (pursuit == null) {
				pursuit = getPursuit();
			}
			
			if (!isOnPursuit && pursuit != null) {
				Game.DisplayNotification("Start pursuit");
				isOnPursuit = true;
			} else if (isOnPursuit && pursuit == null) {
				Game.DisplayNotification("Stop pursuit");
				isOnPursuit = false;
			} else if (isOnPursuit && pursuit != null) {
				Ped[] peds = Functions.GetPursuitPeds(pursuit);
						
				if (peds.Length > 0) {
					
					Ped nearestPed = getNearestSuspect(peds);
					
					if (nearestPed != null) {
						String currentStreetName = World.GetStreetName(World.GetStreetHash(nearestPed.Position));
						if (!lastStreetName.Equals(currentStreetName) && !String.IsNullOrEmpty(currentStreetName)) {
							lastStreetName = currentStreetName;
							Game.DisplayNotification(lastStreetName);
							Functions.PlayScannerAudioUsingPosition("SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", peds[0].Position);
						}
					}
					
				}
			}
		}
		
		private LHandle getPursuit()
		{
			try {
				return Functions.GetActivePursuit();
			} catch (Exception) {
				Game.LogTrivial("fail");
			}
			
			return null;
		}
		
		private Ped getNearestSuspect(Ped[] suspects)
		{			
			for (int i = 0; i < suspects.Length; i++) {
				Ped currentPed = suspects[i];
				
				if (!currentPed.Exists() || Functions.IsPedArrested(currentPed) || Functions.IsPedGettingArrested(currentPed) || Functions.IsPedInPrison(currentPed) || Functions.IsPedStoppedByPlayer(currentPed) || currentPed.IsDead) {
					suspects = RemoveAt(suspects, i);
				}
			}
						
			Array.Sort(suspects, delegate(Ped ped1, Ped ped2) {
				if (ped1.DistanceTo(Game.LocalPlayer.Character) <= ped2.DistanceTo(Game.LocalPlayer.Character)) {
					return -1;
				} else if (ped2.DistanceTo(Game.LocalPlayer.Character) <= ped1.DistanceTo(Game.LocalPlayer.Character)) {
					return 1;
				}
			           	
				return 0;
			});
			
			return suspects.Length < 1 ? null : suspects[0];
		}
		
		private T[] RemoveAt<T>(T[] source, int index)
		{
			T[] dest = new T[source.Length - 1];
			if (index > 0)
				Array.Copy(source, 0, dest, 0, index);

			if (index < source.Length - 1)
				Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

			return dest;
		}
		
	}
}
