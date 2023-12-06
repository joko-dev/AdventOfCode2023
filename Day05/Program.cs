using AdventOfCode2022.SharedKernel;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 5: If You Give A Seed A Fertilizer"));
            Console.WriteLine("Almanac: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            extractMappings(puzzleInput.Lines, out List<Int64> seeds, out List<Mapping> seedToSoil, out List<Mapping> soilToFertilizer, out List<Mapping> fertilizerToWater, out List<Mapping> waterToLight,
                                out List<Mapping> lightToTemperature, out List<Mapping> temperatureToHumidity, out List<Mapping> humidityToLocation);

            List<Int64> locationNumbers = determineLocationNumbers(seeds, seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation);
            Console.WriteLine("Smallest location number: {0}", locationNumbers.Min());

            locationNumbers = determineLocationNumberSeedRange(seeds, seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation);
            Console.WriteLine("Smallest location number: {0}", locationNumbers.Min());
        }

        private static List<Int64> determineLocationNumberSeedRange(List<Int64> seedsRange, List<Mapping> seedToSoil, List<Mapping> soilToFertilizer, List<Mapping> fertilizerToWater, List<Mapping> waterToLight, List<Mapping> lightToTemperature, List<Mapping> temperatureToHumidity, List<Mapping> humidityToLocation)
        {
            List<Int64> locationNumbers = new List<Int64>();

            for (int i = 0; i < seedsRange.Count; i=i+2)
            {
                List<Int64> seeds = new List<Int64>();
                for(Int64 seed = seedsRange[i]; seed < seedsRange[i] + seedsRange[i + 1]; seed++)
                {
                    seeds.Add(seed);
                }
                locationNumbers.Add(determineLocationNumbers(seeds, seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation).Min());
            }

            return locationNumbers;
        }

        private static List<Int64> determineLocationNumbers(List<Int64> seeds, List<Mapping> seedToSoil, List<Mapping> soilToFertilizer, List<Mapping> fertilizerToWater, List<Mapping> waterToLight,
                                            List<Mapping> lightToTemperature, List<Mapping> temperatureToHumidity, List<Mapping> humidityToLocation)
        {
            List<Int64> locationNumbers = new List<Int64>();

            foreach (Int64 seed in seeds)
            {
                Int64 soil = getMappingValue(seed, seedToSoil);
                Int64 fertilizer = getMappingValue(soil, soilToFertilizer);
                Int64 water = getMappingValue(fertilizer, fertilizerToWater);
                Int64 light = getMappingValue(water, waterToLight);
                Int64 temperature = getMappingValue(light, lightToTemperature);
                Int64 humidity = getMappingValue(temperature, temperatureToHumidity);
                Int64 location = getMappingValue(humidity, humidityToLocation);

                locationNumbers.Add(location);
            }

            return locationNumbers;
        }

        private static void extractMappings(List<string> lines, out List<Int64> seeds, out List<Mapping> seedToSoil, out List<Mapping> soilToFertilizer, out List<Mapping> fertilizerToWater, out List<Mapping> waterToLight,
                                            out List<Mapping> lightToTemperature, out List<Mapping> temperatureToHumidity, out List<Mapping> humidityToLocation)
        {
            seeds = extractSeeds(lines[0]);
            seedToSoil = extractMappings(lines, "seed-to-soil map");
            soilToFertilizer = extractMappings(lines, "soil-to-fertilizer map");
            fertilizerToWater = extractMappings(lines, "fertilizer-to-water map");
            waterToLight = extractMappings(lines, "water-to-light map");
            lightToTemperature = extractMappings(lines, "light-to-temperature map");
            temperatureToHumidity = extractMappings(lines, "temperature-to-humidity map");
            humidityToLocation = extractMappings(lines, "humidity-to-location map");                      
        }

        private static List<Mapping> extractMappings(List<string> lines, string map)
        {
            List<Mapping> mappings = new List<Mapping>();
            int lineIndex = lines.FindIndex(line => line.StartsWith(map));
            if (lineIndex == -1) { throw new ArgumentException(); }

            lineIndex++;
            while (lineIndex < lines.Count && lines[lineIndex].Trim() != "")
            {
                string[] items = lines[lineIndex].Split(' ');

                mappings.Add(new Mapping(Int64.Parse(items[0]), Int64.Parse(items[1]), Int64.Parse(items[2])));

                lineIndex++;
            }

            return mappings;
        }

        private static List<Int64> extractSeeds(string line)
        {
            line = line.Replace("seeds: ", "");
            line = line.Trim();

            return line.Split(' ').Select(n => Int64.Parse(n)).ToList();
        }

        private static Int64 getMappingValue(Int64 source, List<Mapping> mapping)
        {
            Int64 target = source;

            foreach (Mapping map in mapping)
            {
                if (map.SourceRangeStart <= source && source <= map.SourceRangeStart + map.RangeLength)
                {
                    target = map.DestinationRangeStart + (source - map.SourceRangeStart);
                    break;
                }
            }

            return target;
        }
    }
}