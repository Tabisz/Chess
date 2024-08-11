using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace _Scripts.Units
{
    public enum Fraction
    {
        PLAYER,
        ENEMY
    }
    public class Unit : MonoBehaviour, ITileOccupier
    {
    
        private UnitsController _playerUnitsController;
        public UnitStatistics Statistics => _statistics;    
        [SerializeField] private UnitStatistics _statistics;
    
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
            _playerUnitsController = unitsController;
            _playerUnitsController.RegisterToUnitsController(this);
            _fraction = fraction;
        
            _statistics = stats;
            _currentHp = _statistics.MaxHp;
        
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

        public void SkipTurn()
        {
            sleepIndicator.SetActive(true);
            _attackPerformed = true;
            _movePerformed = true;        
        }

        public void ResetTurn()
        {
            sleepIndicator.SetActive(false);
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
        public void MoveToTile(Tile tile , Action OnPerformed = null )
        {
            if(_currentTile != null)
                _currentTile.UnregisterOccupier();
        
            StartCoroutine(SmoothLerp(_statistics.MoveSpeed,GameController.Instance.GameplayRefsHolder.GridManager.GetPositionOfTile(tile), OnPerformed));

            _currentTile = tile;
            _movePerformed = true;
            if(tile.CurrentTileOccipier == null)
                tile.RegisterOccupier(this);
        }
    
    
        public void Attack( Unit unit, Action OnPerformed = null)
        {
            Debug.Log("ATTACK!!");
            StartCoroutine(SmoothLerpPingPong(_statistics.AttackSpeed,GameController.Instance.GameplayRefsHolder.GridManager.GetPositionOfTile(unit.CurrentTile), OnPerformed ));
            unit.ReceiveDamage(_statistics.Dmg);
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
            _playerUnitsController.UnRegisterToUnitsController(this);
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
    
        private IEnumerator SmoothLerp (float time, Vector2 finalPos, Action OnPerformed = null)
        {
            GameController.Instance.GameplayRefsHolder.Player.PlayerInputLocker.AddInputLocker(this.name+"move");

            Vector2 startingPos  = transform.position;
            float elapsedTime = 0;
             
            while (elapsedTime < time)
            {
                transform.position = Vector2.Lerp(startingPos, finalPos, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            OnPerformed?.Invoke();
            GameController.Instance.GameplayRefsHolder.Player.PlayerInputLocker.RemoveInputLocker(this.name+"move");

        }
    
        private IEnumerator SmoothLerpPingPong (float time, Vector2 finalPos, Action OnPerformed = null)
        {
            GameController.Instance.GameplayRefsHolder.Player.PlayerInputLocker.AddInputLocker(this.name+"attack");

            Vector2 startingPos  = transform.position;
            float elapsedTime = 0;
             
            while (elapsedTime < time)
            {
                transform.position = Vector2.Lerp(startingPos, finalPos, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0;
            while (elapsedTime < time)
            {
                transform.position = Vector2.Lerp(finalPos, startingPos, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            OnPerformed?.Invoke();
            sleepIndicator.SetActive(true);
            GameController.Instance.GameplayRefsHolder.Player.PlayerInputLocker.RemoveInputLocker(this.name+"attack");

        }
    }
}