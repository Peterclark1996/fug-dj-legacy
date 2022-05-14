import { useAuth0 } from '@auth0/auth0-react'
import { createContext, useContext, useEffect, useState } from 'react'
import MediaData from '../Types/MediaData'
import MediaPlayedData from '../Types/MediaPlayedData'
import NextMediaEvent from '../Types/NextMediaEvent'
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

    const removeFromQueue = (media: MediaData) => {
        setQueue(queue.filter(m => m.player !== media.player || m.code !== media.code))

        if (queue.length > 1 && queue[0].player === media.player && queue[0].code === media.code) {
            connection?.send('QueueMedia', connectedRoomId, queue[1].player, queue[1].code)
        }
    }

    useEffect(() => {
        const nextInQueue = queue[0]

        if (!currentlyPlaying || !nextInQueue) return
        if (currentlyPlaying.userId !== user?.sub) return
        if (currentlyPlaying.player != nextInQueue.player || currentlyPlaying.code != nextInQueue.code) return

        setQueue(queue.slice(1))
        connection?.send('QueueMedia', connectedRoomId, nextInQueue.player, nextInQueue.code)
    }, [connectedRoomId, connection, currentlyPlaying, queue, user?.sub])

    useEffect(() => {
        if (connection === undefined) return
        connection.on('NextMedia', (nextMedia: NextMediaEvent) => setCurrentlyPlaying(nextMedia.upNext || undefined))
    }, [connection])

    const value = {
        queue,
        addToQueue,
        removeFromQueue,
        currentlyPlaying
    }

    return <MediaQueueContext.Provider value={ value } {...props } />
}

export const useMediaQueue = () => useContext(MediaQueueContext)