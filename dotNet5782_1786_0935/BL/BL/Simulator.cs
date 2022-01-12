using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BL
{
    internal class Simulator
    {
        private const double VELOCITY = 1.0;
        private const int DELAY = 1000;
        private const double TIME_STEP = DELAY / 1000;
        private const double STEP = VELOCITY / TIME_STEP;
        public Simulator(int droneId, Action updateDrone, Func<bool> isRun, BL bl)
        {
            BO.Drone drone = new BO.Drone();
            drone = bl.getDrone(droneId);
            while (!isRun())
            {
                if (drone.droneStatus == BO.DroneStatus.available)
                {
                    try
                    {
                        bl.matchDroneWithPacrel(droneId);
                        updateDrone();
                        Thread.Sleep(DELAY);
                        bl.pickUpParcel(droneId);
                        updateDrone();
                        Thread.Sleep(DELAY);
                        bl.deliveredParcel(droneId);
                        updateDrone();
                        Thread.Sleep(DELAY);
                    }
                    catch
                    {
                        bl.SendDroneToCharge(droneId);
                        while (drone.battery < 100)
                        {
                            Thread.Sleep(DELAY);
                            bl.releaseDroneFromCharge(droneId);        
                            drone = bl.getDrone(droneId);
                            bl.SendDroneToCharge(droneId);
                            updateDrone();
                            drone = bl.getDrone(droneId);
                        }
                        bl.releaseDroneFromCharge(droneId);
                        drone = bl.getDrone(droneId);
                        updateDrone();
                        Thread.Sleep(DELAY);
                    }
                    drone = bl.getDrone(droneId);
                }
                if (drone.droneStatus == BO.DroneStatus.maintenance)
                {
                    bl.SendDroneToCharge(droneId);
                    while (drone.battery < 100)
                    {
                        Thread.Sleep(DELAY);
                        bl.releaseDroneFromCharge(droneId);        
                        drone = bl.getDrone(droneId);
                        bl.SendDroneToCharge(droneId);
                        updateDrone();
                        drone = bl.getDrone(droneId);
                    }
                    bl.releaseDroneFromCharge(droneId);
                    drone = bl.getDrone(droneId);
                    updateDrone();
                    Thread.Sleep(DELAY);
                }
                if (drone.droneStatus == BO.DroneStatus.delivery)
                {
                    if (drone.parcel.parcelStatus == false)
                    {
                        bl.pickUpParcel(droneId);
                        updateDrone();
                        Thread.Sleep(DELAY);
                    }
                    bl.deliveredParcel(droneId);
                    updateDrone();
                    Thread.Sleep(DELAY);
                }
                drone = bl.getDrone(droneId);
            }
        }
    }
}
