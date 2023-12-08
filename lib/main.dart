import 'package:advent_of_code/2023/day_1/day_1.dart';
import 'package:advent_of_code/2023/day_2/day_2.dart';
import 'package:advent_of_code/2023/day_3/day_3.dart';
import 'package:advent_of_code/2023/day_4/day_4.dart';
import 'package:advent_of_code/2023/day_5/day_5.dart';
import 'package:advent_of_code/2023/day_6/day_6.dart';
import 'package:advent_of_code/2023/day_7/day_7.dart';
import 'package:advent_of_code/2023/day_8/day_8.dart';

Future<void> main() async {
  var day = Day8();
  print("Day 8");
  var stopwatch = Stopwatch()..start();

  print(await day.solve(isPart2: false));
  print("Part 1 took ${stopwatch.elapsedMilliseconds}ms\n");

  stopwatch.reset();
  stopwatch.start();

  print(await day.solve(isPart2: true));
  print("Part 2 took ${stopwatch.elapsedMilliseconds}ms\n");
}
