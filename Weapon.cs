 class Weapon
    {
        private int _damage;
        private int _bullets;
        private int _countShoot;

        public Weapon(int damage, int bullets, int countShoot)
        {
            _damage = SetOption(damage);
            _bullets = SetOption(bullets);
            _countShoot = 1;
        }

        public void Fire(Player player)
        {
            if(_bullets >= 1)
                player.TakeDamage(_damage);

            int remainderBullets = _bullets - _countShoot;
            if(remainderBullets >= 0)
                _bullets = remainderBullets;
        }

        private int SetOption(int newValue)
        {
            int defaultValue = 0;

            if (newValue >= 0)
                return newValue;
            else
                return defaultValue;
        }
    }

    class Player
    {
        private int _health;

        public Player(int health)
        {
            if (health > 0)
                _health = health;
            else
                _health = 1;
        }

        public void TakeDamage(int damage)
        {
            if (damage >= 0)
                _health -= damage;

            CheakDead();
        }

        public void CheakDead()
        {
            if (_health <= 0)
                IsDied?.Invoke();
        }
    }

    class Bot
    {
        private Weapon _weapon;

        public void OnSeePlayer(Player player)
        {
            if(player != null)
                _weapon.Fire(player);
        }
    }
