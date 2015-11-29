﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StopGuessing.Models;
using System.IO;

namespace Simulator
{


    public class Program
    {
        private const ulong Thousand = 1000;
        private const ulong Million = Thousand * Thousand;
        private const ulong Billion = Thousand * Million;

        public async Task Main(string[] args)
        {
            await Simulator.RunExperimentalSweep((config) =>
            {
                // Scale of test
                ulong totalLoginAttempts = 500*Thousand; // * Million;

                // Figure out parameters from scale
                double meanNumberOfLoginsPerBenignAccountDuringExperiment = 10d;
                double meanNumberOfLoginsPerAttackerControlledIP = 100d;

                double fractionOfLoginAttemptsFromAttacker = 0.5d;
                double fractionOfLoginAttemptsFromBenign = 1d - fractionOfLoginAttemptsFromAttacker;

                double expectedNumberOfBenignAttempts = totalLoginAttempts*fractionOfLoginAttemptsFromBenign;
                double numberOfBenignAccounts = expectedNumberOfBenignAttempts/
                                                meanNumberOfLoginsPerBenignAccountDuringExperiment;

                double expectedNumberOfAttackAttempts = totalLoginAttempts*fractionOfLoginAttemptsFromAttacker;
                double numberOfAttackerIps = expectedNumberOfAttackAttempts/
                                             meanNumberOfLoginsPerAttackerControlledIP;

                // Make any changes to the config or the config.BlockingOptions within config here
                config.TotalLoginAttemptsToIssue = totalLoginAttempts;

                config.FractionOfLoginAttemptsFromAttacker = fractionOfLoginAttemptsFromAttacker;
                config.NumberOfBenignAccounts = (uint) numberOfBenignAccounts;

                // Scale of attackers resources
                config.NumberOfIpAddressesControlledByAttacker = (uint)numberOfAttackerIps;
                config.NumberOfAttackerControlledAccounts = (uint)numberOfAttackerIps;

                // Additional sources of false positives/negatives
                config.FractionOfBenignIPsBehindProxies = 0.1d;
                config.ProxySizeInUniqueClientIPs = 1000;
                config.FractionOfMaliciousIPsToOverlapWithBenign = 0.1;

                //config.BlockingOptions.NumberOfSuccessesToTrackPerIp = 15;
                //config.BlockingOptions.NumberOfFailuresToTrackPerIp = 50;

        // Blocking parameters
        // Make typos almost entirely ignored
        config.BlockingOptions.PenaltyMulitiplierForTypo = 0.1d;
            });



        }
    }
}
