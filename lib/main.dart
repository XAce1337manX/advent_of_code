import 'package:advent_of_code/2023/day_1/day_1.dart';
import 'package:advent_of_code/2023/day_2/day_2.dart';
import 'package:advent_of_code/2023/day_3/day_3.dart';
import 'package:advent_of_code/2023/day_4/day_4.dart';
import 'package:advent_of_code/2023/day_5/day_5.dart';
import 'package:advent_of_code/2023/day_6/day_6.dart';
import 'package:advent_of_code/2023/day_7/day_7.dart';
import 'package:advent_of_code/2023/day_8/day_8.dart';
import 'package:advent_of_code/2023/day_9/day_9.dart';
import 'package:advent_of_code/2023/day_10/day_10.dart';
import 'package:advent_of_code/2023/day_11/day_11.dart';
import 'package:advent_of_code/2023/day_12/day_12.dart';
import 'package:advent_of_code/2023/day_13/day_13.dart';
import 'package:advent_of_code/2023/day_14/day_14.dart';
import 'package:advent_of_code/2023/day_15/day_15.dart';
import 'package:advent_of_code/2023/day_16/day_16.dart';

Future<void> main() async {
  var day = Day16();
  print("Day 16");
  var stopwatch = Stopwatch()..start();

  print(await day.solve(isPart2: false));
  print("Part 1 took ${stopwatch.elapsedMilliseconds}ms\n");

  stopwatch.reset();
  stopwatch.start();

  print(await day.solve(isPart2: true));
  print("Part 2 took ${stopwatch.elapsedMilliseconds}ms\n");
}
