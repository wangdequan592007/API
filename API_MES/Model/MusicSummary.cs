using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Model
{
    public class MusicSummary
    {
        /// <summary>
        /// 歌曲id
        /// </summary>
        [JsonProperty("songId")]
        public int SongId { get; set; }
        /// <summary>
        /// 歌曲名称
        /// </summary>
        [JsonProperty("songName")]
        public string SongName { get; set; }
        /// <summary>
        /// 所属专辑
        /// </summary>
        [JsonProperty("albumName")]
        public string AlbumName { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [JsonProperty("artistName")]
        public string ArtistName { get; set; }
    }
}
