using System.Collections.Generic;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Managers;
using Kugushev.Scripts.Game.Missions.Player;
using Kugushev.Scripts.Presentation.Components.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Components
{
    public class OrdersPresenter : BaseComponent<OrdersManager>
    {
        [SerializeField] private GameObject waypointPrefab;

        private readonly Queue<GameObject> _waypointsPool = new Queue<GameObject>(GameConstants.OrderPathCapacity);
        private readonly List<GameObject> _displayedWaypoints = new List<GameObject>(GameConstants.OrderPathCapacity);

        protected override void OnAwake()
        {
        }

        private void Update()
        {
            HandlePathChanges();
        }

        private void HandlePathChanges()
        {
            var currentOrder = Model.CurrentOrder;
            if (currentOrder?.Status == OrderStatus.Assignment)
            {
                DisplayWaypoints(currentOrder);
            }
            else
                CleanupPath();
        }

        private void DisplayWaypoints(Order currentOrder)
        {
            if (currentOrder.Path.Count > _displayedWaypoints.Count)
            {
                for (var i = _displayedWaypoints.Count; i < currentOrder.Path.Count; i++)
                {
                    var position = currentOrder.Path[i];
                    var waypoint = GetWaypoint(position);
                    _displayedWaypoints.Add(waypoint);
                }
            }
        }

        private void CleanupPath()
        {
            if (_displayedWaypoints.Count > 0)
            {
                foreach (var waypoint in _displayedWaypoints)
                    ReturnWaypoint(waypoint);
                _displayedWaypoints.Clear();
            }
        }

        private GameObject GetWaypoint(Vector3 position)
        {
            var waypoint = _waypointsPool.Count > 0
                ? _waypointsPool.Dequeue()
                : Instantiate(waypointPrefab, transform);

            waypoint.transform.position = position;
            waypoint.SetActive(true);

            return waypoint;
        }

        private void ReturnWaypoint(GameObject waypoint)
        {
            waypoint.SetActive(false);
            _waypointsPool.Enqueue(waypoint);
        }
    }
}