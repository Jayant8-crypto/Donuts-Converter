using Filee.App.Services.Triggers;
using Filee.Core.Settings;

namespace Filee.App.Tests;

public class ShakeGestureDetectorTests
{
    private static ShakeGestureDetector Create() => new(minimumStroke: 32, maximumDurationTicks: 900, requiredReversals: 3);

    [Fact]
    public void Three_fast_reversals_trigger_once_during_a_left_drag()
    {
        var detector = Create();
        detector.Begin(eligible: true, 0, 0);

        Assert.False(detector.Move(40, 0, TriggerMouseButton.Left, 0));
        Assert.False(detector.Move(0, 0, TriggerMouseButton.Left, 100));
        Assert.False(detector.Move(40, 0, TriggerMouseButton.Left, 200));
        Assert.True(detector.Move(0, 0, TriggerMouseButton.Left, 300));
        Assert.False(detector.Move(40, 0, TriggerMouseButton.Left, 400));
    }

    [Fact]
    public void Accidental_movement_without_enough_reversals_does_not_trigger()
    {
        var detector = Create();
        detector.Begin(eligible: true, 0, 0);

        Assert.False(detector.Move(40, 0, TriggerMouseButton.Left, 0));
        Assert.False(detector.Move(0, 0, TriggerMouseButton.Left, 100));
        Assert.False(detector.Move(40, 0, TriggerMouseButton.Left, 200));
    }

    [Fact]
    public void A_slow_shake_must_restart_within_the_gesture_window()
    {
        var detector = Create();
        detector.Begin(eligible: true, 0, 0);

        Assert.False(detector.Move(40, 0, TriggerMouseButton.Left, 0));
        Assert.False(detector.Move(0, 0, TriggerMouseButton.Left, 901));
        Assert.False(detector.Move(40, 0, TriggerMouseButton.Left, 1000));
    }

    [Fact]
    public void Only_eligible_left_button_drags_can_trigger()
    {
        var detector = Create();
        detector.Begin(eligible: false, 0, 0);
        Assert.False(detector.Move(40, 0, TriggerMouseButton.Left, 0));

        detector.Begin(eligible: true, 0, 0);
        Assert.False(detector.Move(40, 0, TriggerMouseButton.Right, 0));
        Assert.False(detector.Move(0, 0, TriggerMouseButton.Left, 100));
        Assert.False(detector.Move(40, 0, TriggerMouseButton.Left, 200));
    }

    [Fact]
    public void A_vertical_shake_is_detected_and_release_cancels_the_sequence()
    {
        var detector = Create();
        detector.Begin(eligible: true, 0, 0);

        Assert.False(detector.Move(0, 40, TriggerMouseButton.Left, 0));
        detector.End();
        Assert.False(detector.Move(0, 0, TriggerMouseButton.Left, 100));
    }
}
