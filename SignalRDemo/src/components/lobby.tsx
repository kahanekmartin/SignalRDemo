import { useState } from 'react';
import { Form, Button } from 'react-bootstrap';

interface Props
{
    logInUrl: string
}

const Lobby = (props: Props) => {
    const [user, setUser] = useState('');

    const logIn = (user: string) =>
    {
        console.log(user)

        fetch(props.logInUrl, {
            method: 'POST',
            headers: {
              Accept: 'application.json',
              'Content-Type': 'application/json'
            },
            body: JSON.stringify({ 'name': user })
          })
    }

    return <Form className='lobby'>
        <Form.Group>
            <Form.Control placeholder="name" onChange={e => setUser(e.target.value)} />
        </Form.Group>
        <Button variant="success" type="submit" disabled={user.length != 0} onClick={() => logIn(user)}>Join</Button>
    </Form>
}

export default Lobby;
