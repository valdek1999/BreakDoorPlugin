using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Exiled.API.Extensions;
using Exiled.API.Interfaces;
using Exiled.Events;
using Handlers = Exiled.Events.Handlers;
using UnityEngine;
using Exiled.Events.EventArgs;
using Random = System.Random;
using MEC;
using Object = UnityEngine.Object;

namespace BreakDoorPlugin
{
    public class EventHandlers: MonoBehaviour
    {
        readonly public Plugin plugin;
        Random r = new Random();
        string[] unbreakableDoorNames = { "079_FIRST", "079_SECOND", "372", "914", "CHECKPOINT_ENT", "CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B", "GATE_A", "GATE_B", "SURFACE_GATE", "ESCAPE", "ESCAPE_INNER"};
        string[] unbreakableNames = { "PrisonDoor", "PrisonDoor (1)", "PrisonDoor (2)", "PrisonDoor (3)", "PrisonDoor (4)", "PrisonDoor (5)", "PrisonDoor (6)", "PrisonDoor (7)", "PrisonDoor (8)", "PrisonDoor (9)", "PrisonDoor (10)", "PrisonDoor (11)", "PrisonDoor (12)", "PrisonDoor (13)", "PrisonDoor (14)", "ContDoor", "Airlocks" };
        List<Door> list_door;
        internal void RoundStart()
        {
            list_door = new List<Door>();
            int k = 0;
            foreach(var t in Object.FindObjectsOfType<Door>())
            {
                if (!unbreakableDoorNames.Contains(t.DoorName) && !unbreakableNames.Contains(t.name))
                    k++;
            }
            Log.Info("количество дверей которые могут взорваться: " + k);
            k = (int)(k * plugin.Config.PercentDoor/100);
            if (k == 0)
                k++;
            Log.Info("количество зачарованных дверей: " + k);
            Door[] items = Object.FindObjectsOfType<Door>();
            for (int i = items.Length - 1; i >= 1; i--)
            {
                int j = r.Next(i + 1);
                // обменять значения data[j] и data[i]
                Door temp = items[j];
                items[j] = items[i];
                items[i] = temp;
            }
            foreach(var t in items)
            {
                if (k == 0)
                    break;
                k--;
                if (!unbreakableDoorNames.Contains(t.DoorName) && !unbreakableNames.Contains(t.name))
                    list_door.Add(t);
            }    
            
        }

        int[] unbreakableTeam = { 0 } ;
        int cooldown;
       

        Dictionary<InteractingDoorEventArgs, bool > temp = new Dictionary<InteractingDoorEventArgs,bool >();
        public EventHandlers(Plugin plugin)
        {
            this.plugin = plugin;
            this.unbreakableTeam = plugin.Config.RoleException;
          
            
        }

        internal void TryBreakDoor(InteractingDoorEventArgs ev)
        {
            Door door = ev.Door;
            foreach (var t in unbreakableTeam)
                if ((Team)t == ev.Player.Team)
                    return;
            foreach(var t in list_door)
            {
                if (ev.Door.transform.position == t.transform.position)
                    BreakDoor(ev);
            }    
        }


        public void BreakDoor(InteractingDoorEventArgs ev)
        {
            Door door = ev.Door;
            door.Networkdestroyed = true;
            door.DestroyDoor(true);
            door.destroyed = true;
        }
    }
}
