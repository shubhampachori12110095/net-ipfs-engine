﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipfs.CoreApi;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Ipfs.Engine.CoreApi
{
    class SwarmApi : ISwarmApi
    {
        IpfsEngine ipfs;

        static MultiAddress[] defaultFilters = new MultiAddress[]
        {
        };

        public SwarmApi(IpfsEngine ipfs)
        {
            this.ipfs = ipfs;
        }

        public async Task<MultiAddress> AddAddressFilterAsync(MultiAddress address, bool persist = false, CancellationToken cancel = default(CancellationToken))
        {
            var addrs = (await ListAddressFiltersAsync(persist, cancel)).ToList();
            if (addrs.Any(a => a == address))
                return address;

            addrs.Add(address);
            var strings = addrs.Select(a => a.ToString());
            await ipfs.Config.SetAsync("Swarm.AddrFilters", JToken.FromObject(strings), cancel);

            // TODO: Tell the swarm
            return address;
        }

        public Task<IEnumerable<Peer>> AddressesAsync(CancellationToken cancel = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync(MultiAddress address, CancellationToken cancel = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DisconnectAsync(MultiAddress address, CancellationToken cancel = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MultiAddress>> ListAddressFiltersAsync(bool persist = false, CancellationToken cancel = default(CancellationToken))
        {
            try
            {
                var json = await ipfs.Config.GetAsync("Swarm.AddrFilters", cancel);
                if (json == null)
                    return new MultiAddress[0];

                return json.Select(a => new MultiAddress((string)a));
            }
            catch (KeyNotFoundException)
            {
                var strings = defaultFilters.Select(a => a.ToString());
                await ipfs.Config.SetAsync("Swarm.AddrFilters", JToken.FromObject(strings), cancel);
                return defaultFilters;
            }
        }

        public Task<IEnumerable<Peer>> PeersAsync(CancellationToken cancel = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<MultiAddress> RemoveAddressFilterAsync(MultiAddress address, bool persist = false, CancellationToken cancel = default(CancellationToken))
        {
            var addrs = (await ListAddressFiltersAsync(persist, cancel)).ToList();
            if (!addrs.Any(a => a == address))
                return address;

            addrs.Remove(address);
            var strings = addrs.Select(a => a.ToString());
            await ipfs.Config.SetAsync("Swarm.AddrFilters", JToken.FromObject(strings), cancel);

            // TODO: Tell the swarm
            return address;
        }
    }
}