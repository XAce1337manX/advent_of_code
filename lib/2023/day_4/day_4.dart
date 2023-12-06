import 'dart:io';
import 'dart:math';

class Day4 {
  final numberExp = RegExp(r'\d+');

  final knownCopiesByGame = <int, int>{};

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_4/day_4_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    // Recursion :D
    if (isPart2) {
      var cardsTotal = 0;
      for (var i = lines.length - 1; i >= 0; i--) {
        var result = 1 + _copiesFromThisGame(i, lines);
        knownCopiesByGame[i] = result;
        cardsTotal += result;
      }
      return cardsTotal;
    }

    var scoreTotal = 0.0;
    for (var line in lines) {
      var scoringNumbers = _getScoringNumbers(line);
      if (scoringNumbers.isNotEmpty) {
        scoreTotal += pow(2, scoringNumbers.length - 1);
      }
    }

    return scoreTotal;
  }

  Iterable<String?> _getScoringNumbers(String line) {
    var cardSections = line.split(RegExp(r':|\|'));
    var winningNumbers = numberExp.allMatches(cardSections[1]).map((match) => match.group(0));
    var myNumbers = numberExp.allMatches(cardSections[2]).map((match) => match.group(0));

    return myNumbers.where((myNumber) => winningNumbers.contains(myNumber));
  }

  int _copiesFromThisGame(int i, List<String> lines) {
    // Safe guard.
    if (i >= lines.length) {
      return 0;
    }

    // Standard recursion memory
    if (knownCopiesByGame.containsKey(i)) {
      return knownCopiesByGame[i]!;
    }

    var numberOfCopies = 0;
    var scoringNumbers = _getScoringNumbers(lines[i]);
    for (var j = i + 1; j < i + 1 + scoringNumbers.length; j++) {
      numberOfCopies += _copiesFromThisGame(j, lines);
    }

    return numberOfCopies;
  }
}
