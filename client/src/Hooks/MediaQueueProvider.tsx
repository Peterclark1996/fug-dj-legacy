import { useAuth0 } from '@auth0/auth0-react'
import { createContext, useContext, useEffect, useState } from 'react'
import MediaData from '../Types/MediaData'
import MediaPlayedData from '../Types/MediaPlayedData'
import { useRoomHub } from './RoomHubProvider'

interface MediaQueueContextInterface {
    queue: MediaData[],
    addToQueue: (media: MediaData) => void,
    removeFromQueue: (media: MediaData) => void,
    currentlyPlaying: MediaPlayedData | undefined
}

const MediaQueueContext = createContext<MediaQueueContextInterface>({
    queue: [],
    addToQueue: () => undefined,
    removeFromQueue: () => undefined,
    currentlyPlaying: undefined
})

export const MediaQueueProvider = (props: React.PropsWithChildren<unknown>) => {
    const { connection, connectedRoomId } = useRoomHub()
    const { user } = useAuth0()
    const [queue, setQueue] = useState<MediaData[]>([])
    const [currentlyPlaying, setCurrentlyPlaying] = useState<MediaPlayedData | undefined>(undefined)

    const addToQueue = (media: MediaData) => {
        setQueue([...queue.filter(m => m.player !== media.player || m.code !== media.code), media])

        if (queue.length === 0) {
            connection?.send('QueueMedia', connectedRoomId, media.player, media.code)
        }
    }

    const removeFromQueue = (media: MediaData) => setQueue(queue.filter(m => m.player !== media.player || m.code !== media.code))

    const playMedia = (mediaToPlay: MediaPlayedData) => {
        console.log(user, currentlyPlaying, queue)

        setCurrentlyPlaying(mediaToPlay)
    }

    useEffect(() => {
        if (!currentlyPlaying || queue.length === 0) return

        if (user?.sub === currentlyPlaying.userId) {
            const nextInQueue = queue[0]
            if (nextInQueue.player === currentlyPlaying.player && nextInQueue.code === currentlyPlaying.code) {
                setQueue(queue.slice(1))
            }
        }
    }, [currentlyPlaying, queue])


    useEffect(() => {
        if (connection === undefined) return
        connection.on('PlayMedia', playMedia)
    }, [connection])

    const value = {
        queue,
        addToQueue,
        removeFromQueue,
        currentlyPlaying
    }

    return <MediaQueueContext.Provider value={value} {...props} />
}

export const useMediaQueue = () => useContext(MediaQueueContext)