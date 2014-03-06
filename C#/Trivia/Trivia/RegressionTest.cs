//  --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegressionTest.cs" company="Cyrille DUPUYDAUBY">
//   Copyright 2014 Cyrille DUPUYDAUBY
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Trivia
{
    using System.Collections.Generic;

    using NFluent;

    using NUnit.Framework;

    using UglyTrivia;

    [TestFixture]
    public class RegressionTest
    {
        [Test]
        public void BasicRuleTest()
        {
            var game = new Game();

            Check.That(game.isPlayable()).IsFalse();

            game.add("one");

            Check.That(game.isPlayable()).IsFalse();

            game.add("two");

            Check.That(game.isPlayable()).IsTrue();

            Check.That(game.howManyPlayers()).IsEqualTo(2);
        }

        [Test]
        public void RollRelatedTests()
        {
            var game = InitBasicGame();
            game.roll(1);
            Check.That(game.CurrentPlayerLocation()).IsEqualTo(1);
            game.roll(12);
            Check.That(game.CurrentPlayerLocation()).IsEqualTo(1);
            game.roll(1);
            Check.That(game.CurrentPlayerLocation()).IsEqualTo(2);
            game.roll(24);
            Check.That(game.CurrentPlayerLocation()).IsEqualTo(2);

            game.roll(2);
            Check.That(game.CurrentPlayerLocation()).IsEqualTo(4);
        }

        [Test]
        public void CategoryRelatedTest()
        {
            var game = InitBasicGame();

            var cells = new List<string>();

            do
            {
                game.roll(1);
                cells.Add(game.currentCategory());
            }
            while (game.CurrentPlayerLocation() > 0);

            Check.That(cells).ContainsExactly(
            new [] {
                "Sports",
                "Rock",
                "Pop",
                "Science",
                "Sports",
                "Rock",
                "Pop",
                "Science",
                "Sports",
                "Rock",
                "Pop","Science" });
        }

        [Test]
        public void PlayerTurnLogicTest()
        {
            var game = InitBasicGame(); 
            Check.That(game.CurrentPlayerScore()).IsEqualTo(0);

            RollAndAnswerProperly(game);

            game.roll(1);

            RollAndAnswerProperly(game);

            while (game.wasCorrectlyAnswered())
            {
                game.roll(1);
            }
            game.wasCorrectlyAnswered();
            Check.That(game.CurrentPlayerScore()).IsEqualTo(6);
        }

        private static void RollAndAnswerProperly(Game game)
        {
            game.roll(1);
            game.wasCorrectlyAnswered();
        }

        [Test]
        public void QuestionLogic()
        {
            var game = InitBasicGame();

            // player one plays properly
            RollAndAnswerProperly(game);
            // player two plays, don't care
            RollAndAnswerProperly(game);

            Check.That(game.CurrentPlayerScore()).IsEqualTo(1);
            // player one plays and fail
            game.roll(1);
            game.wrongAnswer();

            // player two plays, don't care
            RollAndAnswerProperly(game);
            Check.That(game.CurrentPlayerScore()).IsEqualTo(1);
            // player one plays properly
            game.roll(2);
            game.wasCorrectlyAnswered();
            // player two plays, don't care
            RollAndAnswerProperly(game);

            // still in penalty box, no score update
            Check.That(game.CurrentPlayerScore()).IsEqualTo(1);
            Check.That(game.CurrentPlayerLocation()).IsEqualTo(2);

            // another failed attempt
            // player one plays properly
            game.roll(2);
            game.wasCorrectlyAnswered();
            // player two plays, don't care
            RollAndAnswerProperly(game);

            // still in penalty box, no score update
            Check.That(game.CurrentPlayerScore()).IsEqualTo(1);
            Check.That(game.CurrentPlayerLocation()).IsEqualTo(2);

            // player one plays properly
            RollAndAnswerProperly(game);
            // player two plays, don't care
            RollAndAnswerProperly(game);

            // out of penalty box, score updated
            Check.That(game.CurrentPlayerScore()).IsEqualTo(2);
            Check.That(game.CurrentPlayerLocation()).IsEqualTo(3);
        }

        private static Game InitBasicGame()
        {
            var game = new Game();
            game.add("one");
            game.add("two");
            return game;
        }
    }
}
