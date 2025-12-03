import 'dart:io';
import 'dart:math';

import 'package:collection/collection.dart';

class Day7 {
  final numberExp = RegExp(r'\d+');

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_7/day_7_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var camelhands = lines.map((line) => _CamelHand(line, isPart2)).toList();
    camelhands.sort((b, a) => a.compareTo(b));

    var total = 0;
    for (var i = 0; i < camelhands.length; i++) {
      total += (i + 1) * camelhands[i].bet;
    }
    return total;
  }
}

enum _HandType {
  fiveOfAKind,
  fourOfAKind,
  fullHouse,
  threeOfAKind,
  twoPair,
  onePair,
  none;

  int get order => index;
}

class _CamelHand implements Comparable<_CamelHand> {
  final bool isPart2;
  late final String hand;
  late final int bet;

  late final _HandType handType;

  _CamelHand(String line, this.isPart2) {
    var split = line.split(' ');
    hand = split[0];
    bet = int.parse(split[1]);
    handType = _getHandType(hand);
  }

  _HandType _getHandType(String line) {
    var countByCards = <int, int>{};
    for (var codeUnit in hand.codeUnits) {
      countByCards.containsKey(codeUnit)
          ? countByCards[codeUnit] = countByCards[codeUnit]! + 1
          : countByCards[codeUnit] = 1;
    }

    int maxFrequency, uniqueCardsTotal;
    var jokerCodeUnit = 'J'.codeUnits.first;
    var jokerCount = isPart2 ? countByCards.remove(jokerCodeUnit) : null;
    if (jokerCount == null) {
      maxFrequency = countByCards.values.max;
      uniqueCardsTotal = countByCards.keys.length;
    } else if (jokerCount == 5) {
      maxFrequency = 5;
      uniqueCardsTotal = 1;
    } else {
      maxFrequency = countByCards.values.max + jokerCount;
      uniqueCardsTotal = max(1, countByCards.keys.length);
    }

    if (uniqueCardsTotal == 1) {
      return _HandType.fiveOfAKind;
    } else if (uniqueCardsTotal == 2) {
      if (maxFrequency == 4) {
        return _HandType.fourOfAKind;
      } else {
        return _HandType.fullHouse;
      }
    } else if (uniqueCardsTotal == 3) {
      if (maxFrequency == 3) {
        return _HandType.threeOfAKind;
      } else {
        return _HandType.twoPair;
      }
    } else if (uniqueCardsTotal == 4) {
      return _HandType.onePair;
    } else {
      return _HandType.none;
    }
  }

  int _cardValue(String char) {
    return switch (char) {
      'A' => 0,
      'K' => 1,
      'Q' => 2,
      'J' => isPart2 ? 13 : 3,
      'T' => 4,
      '9' => 5,
      '8' => 6,
      '7' => 7,
      '6' => 8,
      '5' => 9,
      '4' => 10,
      '3' => 11,
      '2' => 12,
      _ => 999,
    };
  }

  @override
  int compareTo(_CamelHand other) {
    var handTypeOrder = handType.order - other.handType.order;
    if (handTypeOrder != 0) {
      return handTypeOrder;
    }

    for (var i = 0; i < 5; i++) {
      var cardValueOrder = _cardValue(hand[i]) - _cardValue(other.hand[i]);
      if (cardValueOrder != 0) {
        return cardValueOrder;
      }
    }

    return 0;
  }

  @override
  String toString() {
    return "($hand - $bet - ${handType.name})";
  }
}
