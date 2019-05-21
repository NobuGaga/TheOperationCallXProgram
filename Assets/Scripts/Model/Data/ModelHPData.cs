public struct ModelHPData {
    private int max;
    private int current;

    public ModelHPData(int healthPoint) {
        max = healthPoint;
        current = max;
    }

    public bool IsMax {
        get {
            return current >= max;
        }
    }

    public bool IsZero {
        get {
            return current <= 0;
        }
    }

    private void Reset() {
        current = max;
    }

    public float Percent {
        get {
            return (float)current / max;
        }
    }

    public override string ToString() {
        return string.Format("Current HP : {0} / {1}", current, max);
    }

    public static ModelHPData operator +(ModelHPData healthPoint, int value) {
        healthPoint.current += value;
        if (healthPoint.IsMax)
            healthPoint.Reset();
        return healthPoint;
    }

    public static ModelHPData operator -(ModelHPData healthPoint, int value) {
        healthPoint.current -= value;
        if (healthPoint.current < 0)
            healthPoint.current = 0;
        return healthPoint;
    }
}