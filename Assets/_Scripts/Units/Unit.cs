using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Units
{
    public enum Fraction
    {
        PLAYER,
        ENEMY
    }
    public class Unit : MonoBehaviour, ITileOccupier
    {
    
        private UnitsController _unitsController;
        public UnitStatistics Statistics => statistics;    
        [SerializeField] private UnitStatistics statistics;
    
        public int CurrentHp => _currentHp;
        private int _currentHp;

        private bool _movePerformed;
        private bool _attackPerformed;

        public Fraction Fraction => _fraction;
        private Fraction _fraction;

        public Tile CurrentTile => _currentTile;
        private Tile _currentTile;

        [SerializeField] private MeshRenderer myRenderer;

        [SerializeField] private GameObject sleepIndicator;
    
    
        public void Init(UnitsController unitsController, Fraction fraction, UnitStatistics stats, Material graphic, bool disableForNextTurn)
        {
            _unitsController = unitsController;
            _unitsController.RegisterToUnitsController(this);
            _fraction = fraction;
        
            statistics = stats;
            _currentHp = statistics.MaxHp;
        
            myRenderer.material = graphic;
        
            if(disableForNextTurn)
                SkipTurn();
            else
                ResetTurn();
        }

        public bool CanPerformAttack()
        {
            return !_attackPerformed;
        }

        public bool CanPerformMove()
        {
            return !_attackPerformed && !_movePerformed;
        }
        
        public Fraction GetOppositeFraction()
        {
            if (Fraction == Fraction.ENEMY)
                return Fraction.PLAYER;
            else
                return Fraction.ENEMY;
        }

        public void SetWakeUp()
        {
            sleepIndicator.SetActive(false);
        }

        public void SetSleeping()
        {
            sleepIndicator.SetActive(true);
        }

        public void SkipTurn()
        {
            SetSleeping();
            _attackPerformed = true;
            _movePerformed = true;        
        }

        public void ResetTurn()
        {
            SetWakeUp();
            _attackPerformed = false;
            _movePerformed = false;
        }

        public void SpawnAtTile(Tile tile)
        {
            transform.position = GameController.Instance.GameplayRefsHolder.GridManager.GetPositionOfTile(tile);
            _currentTile = tile;
        
            if(tile.CurrentTileOccipier == null)
                tile.RegisterOccupier(this);
        }
        public void MoveToTile(Tile tile , Action onPerformed = null )
        {
            if(_currentTile != null)
                _currentTile.UnregisterOccupier();
        
            StartCoroutine(SmoothLerp(statistics.MoveSpeed,GameController.Instance.GameplayRefsHolder.GridManager.GetPositionOfTile(tile), onPerformed));

            _currentTile = tile;//TODO: move this to on performed function because it is firing to early
            _movePerformed = true;
            if(tile.CurrentTileOccipier == null)
                tile.RegisterOccupier(this);
        }
    
    
        public void Attack( Unit unit, Action onPerformed = null)
        {
            Debug.Log("ATTACK!!");
            StartCoroutine(SmoothLerpPingPong(statistics.AttackSpeed,GameController.Instance.GameplayRefsHolder.GridManager.GetPositionOfTile(unit.CurrentTile), onPerformed ));
            unit.ReceiveDamage(statistics.Dmg);//TODO: move this to on performed function because it is firing to early
            _attackPerformed = true;
        }
        private void ReceiveDamage(int damageCount)
        {
            _currentHp -= damageCount;
            GameController.Instance.GameplayRefsHolder.Observer.OnDamageReceived?.Invoke(this);
        
            if(_currentHp<=0)
                Die();
        }
        private void Die()
        {
            _currentTile.UnregisterOccupier();
            gameObject.SetActive(false);
            _unitsController.UnRegisterToUnitsController(this);
            GameController.Instance.GameplayRefsHolder.Observer.OnUnitDied?.Invoke(this);
        }
    
        //for player unity only
        public void OnMyTileSelected()
        {
            GameController.Instance.GameplayRefsHolder.Observer.OnUnitSelected?.Invoke(this);

        }
        public void OnMyTileSecondarySelected()
        {
            GameController.Instance.GameplayRefsHolder.Observer.OnUnitSecondarySelected?.Invoke(this);
        }

        public void OnMyTileKillCommand()
        {
            if(_fraction == Fraction.PLAYER)
                Die();
        }
    
        private IEnumerator SmoothLerp (float moveSpeed, Vector2 finalPos, Action onPerformed = null)
        {
            GameController.Instance.GameplayRefsHolder.Player.PlayerInputLocker.AddInputLocker(this.name+"move");

            Vector2 startingPos  = transform.position;
            float elapsedTime = 0;

            float moveTime =Vector2.Distance(startingPos,finalPos) / moveSpeed;
            
            while (elapsedTime < moveTime)
            {
                transform.position = Vector2.Lerp(startingPos, finalPos, (elapsedTime / moveTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            onPerformed?.Invoke();
            GameController.Instance.GameplayRefsHolder.Player.PlayerInputLocker.RemoveInputLocker(this.name+"move");

        }
    
        private IEnumerator SmoothLerpPingPong (float moveSpeed, Vector2 finalPos, Action onPerformed = null)
        {
            GameController.Instance.GameplayRefsHolder.Player.PlayerInputLocker.AddInputLocker(this.name+"attack");
            
            Vector2 startingPos  = transform.position;
            float elapsedTime = 0;
             
            float moveTime =Vector2.Distance(startingPos,finalPos) / moveSpeed;
            
            while (elapsedTime < moveTime)
            {
                transform.position = Vector2.Lerp(startingPos, finalPos, (elapsedTime / moveTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            elapsedTime = 0;
            moveTime =Vector2.Distance(startingPos,finalPos) / moveSpeed;
            
            while (elapsedTime < moveTime)
            {
                transform.position = Vector2.Lerp(finalPos, startingPos, (elapsedTime / moveTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            onPerformed?.Invoke();
            GameController.Instance.GameplayRefsHolder.Player.PlayerInputLocker.RemoveInputLocker(this.name+"attack");

        }
    }
}