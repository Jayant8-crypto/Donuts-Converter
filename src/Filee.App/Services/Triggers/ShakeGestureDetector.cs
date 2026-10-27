using Filee.Core.Settings;

namespace Filee.App.Services.Triggers;

/// <summary>Detects deliberate alternating movement while a left-button drag is active.</summary>
internal sealed class ShakeGestureDetector(int minimumStroke, long maximumDurationTicks, int requiredReversals)
{
    private enum Axis { None, Horizontal, Vertical }

    private bool _eligible;
    private bool _triggered;
    private Axis _axis;
    private int _originX, _originY, _extreme, _direction, _reversals;
    private long _startedAt;

    public void Begin(bool eligible, int x, int y)
    {
        _eligible = eligible;
        _triggered = false;
        _axis = Axis.None;
        _originX = x;
        _originY = y;
        _reversals = 0;
    }

    public bool Move(int x, int y, TriggerMouseButton heldButton, long timestamp)
    {
        if (!_eligible || _triggered || heldButton != TriggerMouseButton.Left)
            return false;

        if (_axis == Axis.None)
        {
            var dx = x - _originX;
            var dy = y - _originY;
            if (Math.Max(Math.Abs(dx), Math.Abs(dy)) < minimumStroke)
                return false;

            _axis = Math.Abs(dx) >= Math.Abs(dy) ? Axis.Horizontal : Axis.Vertical;
            var position = _axis == Axis.Horizontal ? x : y;
            var origin = _axis == Axis.Horizontal ? _originX : _originY;
            _direction = Math.Sign(position - origin);
            _extreme = position;
            _startedAt = timestamp;
            return false;
        }

        if (timestamp - _startedAt > maximumDurationTicks)
        {
            Begin(eligible: true, x, y);
            return false;
        }

        var current = _axis == Axis.Horizontal ? x : y;
        var distance = current - _extreme;
        if (Math.Sign(distance) == _direction)
        {
            _extreme = current;
            return false;
        }

        if (Math.Abs(distance) < minimumStroke)
            return false;

        _direction = -_direction;
        _extreme = current;
        if (++_reversals < requiredReversals)
            return false;

        _triggered = true;
        return true;
    }

    public void End() => _eligible = false;
}
