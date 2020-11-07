using System;
using System.Collections;
using MStudios;
using MStudios.Grid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ships.SideControllers
{
    public class AITurnController : MonoBehaviour, IShipSideController
    {
        [SerializeField] private GameObject cannonPrefab;
        [SerializeField] private Transform enemyGridTransform;
        
        private Cannon cannon;

        public event Action OnTurnEnd;
        private Grid2D<ShipCellState> _grid;
        
        private void Awake()
        {
            cannon = Instantiate(cannonPrefab, transform).GetComponent<Cannon>();
            cannon.GetComponent<SpriteRenderer>().enabled = false;
            cannon.transform.localPosition = Vector2.zero;
        }

        public void SetGrid(Grid2D<ShipCellState> grid)
        {
            _grid = grid;
            transform.position = enemyGridTransform.position;
            
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            StartCoroutine(Attack());
        }

        public void Deactivate()
        {
            _grid = null;
            gameObject.SetActive(false);
        }

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(0.3f);

            var hasLiveCell = _grid.HasCellWithValue(ShipCellState.Alive);

            if (hasLiveCell)
            {
                var attackLiveCell = Random.Range(0, 100) > 20;
                Vector3 cell;

                if (attackLiveCell)
                {
                    cell = _grid.GetRandomLocalCellPositionByValue(ShipCellState.Alive).AsVector3();    
                }
                else
                {
                    cell = _grid.GetRandomLocalCellPositionByValue(ShipCellState.Empty).AsVector3();
                }
                
                var cannonPosition = new Vector2(Random.Range(0, _grid.columns), Random.Range(0, _grid.rows));
                cannon.transform.localPosition = cannonPosition;

                var worldPosition = cell + _grid.gridPosition.AsVector3();
            
                cannon.FireAt(worldPosition);    
            }
            
            this.OnTurnEnd?.Invoke();
        }
    }
}