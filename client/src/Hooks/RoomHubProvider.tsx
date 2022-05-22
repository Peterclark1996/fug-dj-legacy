import { useState, useEffect, createContext, useContext } from 'react'
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { useAuth0 } from '@auth0/auth0-react'
import { HubUrl } from '../Constants'
import ConnectedUser from '../Types/ConnectedUser'

const getHubUrl = () => {
    if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
        return HubUrl.DEV
    }
    return HubUrl.PROD
}

interface RoomHubInterface {
    connection: HubConnection | undefined,
    connectedRoomId: string | undefined,
    connectedUsers: ConnectedUser[],
    connectToRoom: (roomId: string) => void,
    disconnectFromRoom: () => void,
}

const RoomHubContect = createContext<RoomHubInterface>({
    connection: undefined,
    connectedRoomId: undefined,
    connectedUsers: [],
    connectToRoom: () => console.error("RoomHubProvider not initialized"),
    disconnectFromRoom: () => console.error("RoomHubProvider not initialized")
})

export const RoomHubProvider = (props: React.PropsWithChildren<unknown>) => {
    const { getAccessTokenSilently } = useAuth0()

    const [connection, setConnection] = useState<HubConnection | undefined>(undefined)
    const [currentRoom, setCurrentRoom] = useState<string | undefined>(undefined)
    const [connectedUsers, setConnectedUsers] = useState<ConnectedUser[]>([])

    const connectToRoom = (roomId: string) => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(getHubUrl(), { accessTokenFactory: async () => await getAccessTokenSilently() })
            .withAutomaticReconnect()
            .build()

        setConnectedUsers([])
        setConnection(newConnection)
        setCurrentRoom(roomId)
    }

    const disconnectFromRoom = () => {
        if (currentRoom === undefined) return
        if (connection === undefined) return

        setConnectedUsers([])
        setCurrentRoom(undefined)
    }

    useEffect(() => {
        if (currentRoom === undefined) return
        if (connection && connection.state === "Disconnected") {
            connection.start()
                .then(async () => await connection.send('JoinRoom', currentRoom))
                .catch(e => console.log('Failed to connect to queue: ', e))
        }
    }, [connection, currentRoom])

    useEffect(() => {
        if (connection === undefined) return
        connection.on('UpdateRoomUsers', (roomUsers: ConnectedUser[]) => {
            if (JSON.stringify(roomUsers) === JSON.stringify(connectedUsers)) return
            setConnectedUsers(roomUsers)
        })
    }, [connectedUsers, connection])

    const value = {
        connection,
        connectedRoomId: currentRoom,
        connectedUsers,
        connectToRoom,
        disconnectFromRoom
    }

    return <RoomHubContect.Provider value={value} {...props} />
}

export const useRoomHub = () => useContext(RoomHubContect)