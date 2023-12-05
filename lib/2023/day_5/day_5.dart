import 'dart:io';

import 'package:collection/collection.dart';

class Day5 {
  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_5/day_5_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var seeds =
        lines[0].substring('seeds: '.length).split(' ').map((seed) => int.parse(seed)).toList();

    var seedMaps = <int, List<_SeedMap>>{};
    var mapIndex = -1;
    for (var i = 1; i < lines.length; i++) {
      var line = lines[i];
      if (line.isEmpty) continue;

      if (line.contains("map")) {
        mapIndex++;
        continue;
      }

      seedMaps.containsKey(mapIndex)
          ? seedMaps[mapIndex]!.add(_SeedMap(line))
          : seedMaps[mapIndex] = [_SeedMap(line)];
    }

    // Naively bruting force part 1 solution for part 2 didn't
    // work but what if I did the map in reverse instead?
    if (isPart2) {
      var seedPairs = <({int startSeedRange, int lengthSeedRange})>[];
      for (var i = 0; i < seeds.length; i += 2) {
        var startSeedRange = seeds[i];
        var lengthSeedRange = seeds[i + 1];

        seedPairs.add((startSeedRange: startSeedRange, lengthSeedRange: lengthSeedRange));
      }

      var lowestTestedlocation = 0;
      // This is an upper bound given by part 1
      while (lowestTestedlocation < 500000000) {
        var location = lowestTestedlocation;
        for (var i = seedMaps.length - 1; i >= 0; i--) {
          for (var seedMap in seedMaps[i]!) {
            if ((seedMap.destinationRangeStart <= location &&
                location < seedMap.destinationRangeStart + seedMap.rangeLength)) {
              location += seedMap.sourceRangeStart - seedMap.destinationRangeStart;
              break;
            }
          }
        }

        for (var seedPair in seedPairs) {
          if (seedPair.startSeedRange <= location &&
              location < seedPair.startSeedRange + seedPair.lengthSeedRange) {
            return lowestTestedlocation;
          }
        }

        lowestTestedlocation++;
      }

      return -1; // Welp it didn't work.
    }

    var locations = <int>[];
    for (var seed in seeds) {
      var location = seed;
      for (var i = 0; i < seedMaps.length; i++) {
        for (var seedMap in seedMaps[i]!) {
          if ((seedMap.sourceRangeStart <= location &&
              location < seedMap.sourceRangeStart + seedMap.rangeLength)) {
            location += seedMap.destinationRangeStart - seedMap.sourceRangeStart;
            break;
          }
        }
      }
      locations.add(location);
    }

    return locations.min;
  }
}

class _SeedMap {
  late final int destinationRangeStart;
  late final int sourceRangeStart;
  late final int rangeLength;

  _SeedMap(String line) {
    var values = line.split(' ');
    destinationRangeStart = int.parse(values[0]);
    sourceRangeStart = int.parse(values[1]);
    rangeLength = int.parse(values[2]);
  }

  @override
  String toString() {
    return "($destinationRangeStart $sourceRangeStart $rangeLength)";
  }
}
