import YouTube, { YouTubeProps } from 'react-youtube'

type YoutubePlayerProps = {
    videoCode: string
}

const YoutubePlayer = ({ videoCode }: YoutubePlayerProps) => {
    const opts: YouTubeProps['opts'] = {
        height: '390',
        width: '640',
        playerVars: {
            autoplay: 1,
            controls: 0,
            disablekb: 1,
            modestbranding: 1,
            fs: 0,
            start: 0
        },
    }

    const onReady: YouTubeProps['onReady'] = event => event.target.playVideo()

    const onPause: YouTubeProps['onPause'] = event => event.target.playVideo()

    return (
        <YouTube videoId={videoCode} opts={opts} onReady={onReady} onPause={onPause} />
    )
}

export default YoutubePlayer