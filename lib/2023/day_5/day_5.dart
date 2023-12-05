import 'dart:io';
import 'dart:math';

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
