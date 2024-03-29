﻿using System;
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

        /// <summary>
        /// This is a simulator that allows the drone to fulfill deliveries autimaticlly.
        /// </summary>
        public Simulator(int droneId, Action updateDrone, Func<bool> isRun, BL bl)  //constructor
        {
            BO.Drone drone = new BO.Drone();
            drone = bl.getDrone(droneId);
            while (!isRun()) //goes untill the manual button is pressed
            {
                if (drone.droneStatus == BO.DroneStatus.available)  //if the drone is available
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
                    catch  //if there is not anough battery to fulfill an order or if there are no available parcels left,
                           //this sends the drone to a chrging station
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
                if (drone.droneStatus == BO.DroneStatus.maintenance) //if the drone is charging, finishes its charge.
                {
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
                if (drone.droneStatus == BO.DroneStatus.delivery)  //if the drone is in the middle of a delivery,
                                                                   //it completes it before looking for new parcels
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
