// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;

namespace osu.Framework.Tests.Visual.Sprites
{
    public class TestSceneRomanisableSpriteText : FrameworkTestScene
    {
        private readonly FillFlowContainer flow;

        private Bindable<bool> showUnicodeBindable;

        public TestSceneRomanisableSpriteText()
        {
            Children = new Drawable[]
            {
                new BasicScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new[]
                    {
                        flow = new FillFlowContainer
                        {
                            Anchor = Anchor.TopLeft,
                            AutoSizeAxes = Axes.Y,
                            RelativeSizeAxes = Axes.X,
                            Direction = FillDirection.Vertical,
                        }
                    }
                }
            };

            flow.Add(new SpriteText { Text = new RomanisableString("ongaku", "music") });
            flow.Add(new SpriteText { Text = new RomanisableString("", "music") });
            flow.Add(new SpriteText { Text = new RomanisableString("ongaku", "") });
        }

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config)
        {
            showUnicodeBindable = config.GetBindable<bool>(FrameworkSetting.ShowUnicode);
        }

        [Test]
        public void TestToggleRomanisedState()
        {
            AddStep("prefer romanised", () => showUnicodeBindable.Value = false);
            AddAssert("check strings correct", () => flow.OfType<SpriteText>().Select(st => st.Current.Value).SequenceEqual(new[] { "music", "music", "ongaku" }));

            AddStep("prefer unicode", () => showUnicodeBindable.Value = true);
            AddAssert("check strings correct", () => flow.OfType<SpriteText>().Select(st => st.Current.Value).SequenceEqual(new[] { "ongaku", "music", "ongaku" }));
        }
    }
}
