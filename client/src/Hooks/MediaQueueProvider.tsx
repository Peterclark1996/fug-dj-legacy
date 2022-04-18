import { createContext, useContext, useState } from 'react'
import MediaData from '../Types/MediaData'
import { useRoomHub } from './RoomHubProvider'

interface MediaQueueContextInterface {
    queue: MediaData[],
    addToQueue: (media: MediaData) => void,
    removeFromQueue: (media: MediaData) => void
}

const MediaQueueContext = createContext<MediaQueueContextInterface>({
    queue: [],
    addToQueue: () => { },
    removeFromQueue: () => { }
})

export const MediaQueueProvider = (props: React.PropsWithChildren<{}>) => {
    const { connection, connectedRoomId } = useRoomHub()
    const [queue, setQueue] = useState<MediaData[]>([])

    const addToQueue = (media: MediaData) => {
        if (queue.length === 0) {
            connection?.send('QueueMedia', connectedRoomId, media.player, media.code)
        }

        setQueue([...queue.filter(m => m.player !== media.player || m.code !== media.code), media])
    }

    const removeFromQueue = (media: MediaData) => setQueue(queue.filter(m => m.player !== media.player || m.code !== media.code))

    const value = {
        queue,
        addToQueue,
        removeFromQueue
    }

    return <MediaQueueContext.Provider value={value} {...props} />
}

export const useMediaQueue = () => useContext(MediaQueueContext)